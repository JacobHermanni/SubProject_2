define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var fpSearchString = ko.observable("");

        var searched = function() {
            bc.publish(bc.events.changeView, { name: "all-posts", fp_msg: fpSearchString()} );
        }

        return {
            searched,
            fpSearchString
        };

    }
});