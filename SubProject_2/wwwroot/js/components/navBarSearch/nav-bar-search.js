define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var navSearchString = ko.observable("");

        var navSearched = function() {
            console.log("navSearched");
            bc.publish(bc.events.changeView, { name: "all-posts", nav_msg: navSearchString()} );
        }

        return {
            navSearched,
            navSearchString
        };

    }
});