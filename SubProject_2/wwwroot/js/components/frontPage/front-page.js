define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var fpSearchString = ko.observable("");

        var searched = function() {
       


            if (fpSearchString().length < 1 || fpSearchString().length > 25) {
                alert("The typed search string is either too long or too short, which one is it?");
            } else {
                bc.publish(bc.events.changeView, { to: "all-posts", from: "front-page", search: fpSearchString()} );
                bc.publish(bc.events.changeData, { search_string: fpSearchString() });
            }
        }

        return {
            searched,
            fpSearchString
        };

    }

});