define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        bc.subscribe(bc.events.changeData,
            dataInfo => {
                console.log("viewData from nav-bar", dataInfo);
                navSearchString(dataInfo.search_string);
            });

        var navSearchString = ko.observable("");

        var navSearched = function() {
            console.log("navSearched");
            bc.publish(bc.events.changeView, { name: "all-posts", nav_msg: navSearchString()} );
        }

        var navBarView = ko.observable('front-page');
        console.log("nav-view:", navBarView());

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
            console.log("nav-view:", navBarView());
            bc.publish(bc.events.changeView, { name: navBarView(), nav_msg: navSearchString() });
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