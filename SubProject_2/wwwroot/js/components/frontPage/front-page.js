define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var fpSearchString = ko.observable("");

        var searched = function() {

            console.log(fpSearchString());


            if (fpSearchString().length < 1 || fpSearchString().length > 25) {
                console.log("field not filled out fucko");
                alert("The typed search string is either too long or too short, which one is it?");
            } else {
                bc.publish(bc.events.changeView, { name: "all-posts", fp_msg: fpSearchString()} );
                bc.publish(bc.events.changeData, { search_string: fpSearchString() });
            }
        }

        return {
            searched,
            fpSearchString
        };

    }

});