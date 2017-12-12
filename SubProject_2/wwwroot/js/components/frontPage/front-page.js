﻿define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var fpSearchString = ko.observable("");

        var searched = function() {
            bc.publish(bc.events.changeView, { name: "all-posts", fp_msg: fpSearchString()} );
            bc.publish(bc.events.changeData, { search_string: fpSearchString() });
        }

        var test = function () {
            bc.publish(bc.events.changeView, { name: "user-page", fp_msg: "13"} );
        }

        return {
            searched,
            fpSearchString,
            test
        };

    }
});