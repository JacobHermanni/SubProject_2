define([], function () {

    var searchedPosts = function (searchString, callback) {
        $.getJSON("http://localhost:5001/api/posts/search/" + searchString, function (data) {
            console.log("searchedPosts on:", searchString, data);
            callback(data);
        })
            .done(function () {
                console.log('getJSON request succeeded!');
            })
            .fail(function () {
                console.log('getJSON request failed!');
                var furb = {
                    data: [{ body: "No search result" }],
                    next: null,
                    prev: null
                };
                console.log('Failed search returns object: ', furb);
                callback(furb);
            })
            .always(function () {
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

    var getAllFavorites = function (callback) {
        $.getJSON("http://localhost:5001/api/favorite?page=0&pageSize=1000", function (data) {
            console.log("got favorites:", data);
            callback(data);
        });
    }

    var postFavorite = function (post_id, callback) {
        var jsonData = { '': post_id };
        console.log("from dataservice. jsonData = ", jsonData);
        $.ajax("http://localhost:5001/api/favorite/", {
            data: JSON.stringify(post_id),
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            dataType: "json",
            success: callback
        });
    }

    var deleteFavorite = function (fav_id, callback) {
        $.ajax("http://localhost:5001/api/favorite/" + fav_id, {
            type: 'DELETE',
            success: callback
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
        getHistory,
        postFavorite,
        deleteFavorite,
        getAllFavorites
    };

});