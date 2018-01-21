define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        bc.subscribe(bc.events.changeData,
            dataInfo => {
                navSearchString(dataInfo.search_string);
            });

        var navSearchString = ko.observable("");

        var navSearched = function () {
            if (navSearchString().length < 1 || navSearchString().length > 25) {
                alert("The typed search string is either too long or too short, which one is it?");
            } else {
                bc.publish(bc.events.changeView, { to: "all-posts", from: "nav-search", search: navSearchString() });
            }

        }

        // changeview from bc event. Change navBarView observable to let it automatically change css for menu items to active/inactive
        bc.subscribe(bc.events.changeView,
            viewInfo => {
                navBarView(viewInfo.to);
            });

        var navBarView = ko.observable('front-page');

        var pages = [
            { name: 'Posts', view: 'all-posts' },
            { name: 'Favorites', view: 'favorites-page' },
            { name: 'History', view: 'history-page' }
        ];

        var isActive = function (menu) {
            if (menu.view === navBarView()) {
                return 'active';
            }
            return '';
        }

        var changeView = function (menu) {
            navBarView(menu.view);
            bc.publish(bc.events.changeView, { to: navBarView(), from: "nav-bar" });
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