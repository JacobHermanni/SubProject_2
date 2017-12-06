define([], function() {

    var searchedPosts = function (searchString, callback) {
        $.getJSON("http://localhost:5001/api/posts/search/" + searchString, function (data) {
            console.log("searchedPosts on:", searchString, data);
            callback(data);
        })
            .done(function()
            {
                console.log('getJSON request succeeded!');
            })
            .fail(function()
            {
                console.log('getJSON request failed!');
                var furb = {
                    data: [ {body: "Ingen søgeresultater"} ],
                    next: null,
                    prev: null
                };
                callback(furb);
            })
            .always(function()
            {
                console.log('getJSON request ended!'); 
            });
    }

    var changePage = function (url, callback) {
        $.getJSON(url, function (data) {
            console.log("changed page");
            callback(data);
        });
    }

    var getQuestion = function (url, callback) {
        $.getJSON(url, function (data) {
            console.log("got question:", data);
            callback(data);
        });
    }

    var getAnswers = function (url, callback) {
        $.getJSON(url, function (data) {
            console.log("got answers:", data);
            callback(data);
        });
    }



    return {
        searchedPosts,
        changePage,
        getQuestion,
        getAnswers
    };

});