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
            console.log("changed page, new data:", data);
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

    var getFavorites = function (callback) {
        $.getJSON("http://localhost:5001/api/favorite/", function (data) {
            console.log("got favorites:", data);
            callback(data);
        });
    }

    var getHistory = function (callback) {
        $.getJSON("http://localhost:5001/api/history/", function (data) {
            console.log("got history:", data);
            callback(data);
        });
    }


    return {
        searchedPosts,
        changePage,
        getQuestion,
        getAnswers,
        getFavorites,
        getHistory
    };

});