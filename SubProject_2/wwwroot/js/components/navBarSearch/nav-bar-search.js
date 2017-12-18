﻿define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        bc.subscribe(bc.events.changeData,
            dataInfo => {
                navSearchString(dataInfo.search_string);
            });

        var navSearchString = ko.observable("");

        var navSearched = function() {
            if (navSearchString().length < 1 || navSearchString().length > 25) {
                alert("The typed search string is either too long or too short, which one is it?");
            } else {
                bc.publish(bc.events.changeView, { name: "all-posts", nav_msg: navSearchString()} );
            }

        }

        var navBarView = ko.observable('front-page');

        var pages = [
            { name: 'Posts', view: 'all-posts' },
            { name: 'Favorites', view: 'favorites-page' },
            { name: 'History', view: 'history-page' }
        ];

        var isActive = function(menu) {
            if(menu.view === navBarView()) {
                return 'active';
            }
            return '';
        }

        var changeView = function(menu) {
            navBarView(menu.view);
            bc.publish(bc.events.changeView, { name: navBarView()});
        }


        return {
            navSearched,
            navSearchString,
            pages,
            isActive,
            changeView
        };

    }
});