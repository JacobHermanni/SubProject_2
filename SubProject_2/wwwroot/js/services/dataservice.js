define([], function () {

    var searchedPosts = function (searchString, callback) {
        $.getJSON("http://localhost:5001/api/posts/search/" + searchString + "?pageSize=10", function (data) {
            callback(data);
        })
            .done(function () {
            })
            .fail(function () {
                var furb = {
                    data: [{ body: "No search result" }],
                    next: null,
                    prev: null
                };
                callback(furb);
            })
            .always(function () {
            });
    }

    var getRelatedWords = function (words, callback) {
        $.getJSON("http://localhost:5001/api/relatedwords/" + words,
            function (data) {
                callback(data);
            })
            .fail(function () {
                callback(undefined);
            });
    }

    var getTermNetwork = function (word, callback) {
        $.getJSON("http://localhost:5001/api/termnetwork/" + word, function (data) {
            if (data !== undefined) {
                callback(data);
            }
        });
    }

    var changePage = function (url, callback) {
        $.getJSON(url, function (data) {
            callback(data);
        });
    }

    var refreshPostsPage = function (url, callback) {
        $.getJSON(url, function (data) {
            console.log("refreshed page, new data:", data);
            callback(data);
        });
    }

    var getQuestion = function (url, callback) {
        $.getJSON(url, function (data) {
            callback(data);
        });
    }

    var getAnswers = function (url, callback) {
        $.getJSON(url, function (data) {
            callback(data);
        });
    }

    var getFavorites = function (callback) {
        $.getJSON("http://localhost:5001/api/favorite?pageSize=10", function (data) {
            callback(data);
        });
    }

    var refreshFavorites = function (url, callback) {
        $.getJSON(url, function (data) {
            console.log("refreshed favorites, new data:", data);
            callback(data);
        });
    }

    var getAllFavorites = function (callback) {
        $.getJSON("http://localhost:5001/api/favorite?page=0&pageSize=1000", function (data) {
            callback(data);
        });
    }

    var postFavorite = function (post_id, callback) {
        var jsonData = { '': post_id };
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
            callback(data);
        });
    }

    var refreshHistory = function (url, callback) {
        $.getJSON(url, function (data) {
            console.log("refreshed history, new data:", data);
            callback(data);
        });
    }

    var getNote = function (fav_id, callback) {
        $.getJSON("http://localhost:5001/api/favorite/note/" + fav_id, function (data) {
            callback(data);
        });
    }

    var postNote = function (fav_id, noteString, callback) {
        var jsonData = JSON.stringify({ favorite_id: fav_id, body: noteString });
        $.ajax("http://localhost:5001/api/favorite/note/", {
            data: jsonData,
            contentType: 'application/json',
            type: 'POST',
            success: callback
        });
    }

    var deleteNote = function (fav_id, callback) {
        $.ajax("http://localhost:5001/api/favorite/note/" + fav_id, {
            type: 'DELETE',
            success: callback
        });
    }

    var putNote = function (fav_id, noteString, callback) {
        var jsonData = JSON.stringify({ favorite_id: fav_id, body: noteString });
        $.ajax("http://localhost:5001/api/favorite/note/", {
            data: jsonData,
            contentType: 'application/json',
            type: 'PUT',
            success: callback
        });
    }

    var getUser = function (user_id, callback) {
        $.getJSON("http://localhost:5001/api/user/" + user_id, function (data) {
            callback(data);
        });
    }

    var getUserPosts = function (user_id, callback) {
        $.getJSON("http://localhost:5001/api/user/userposts/" + user_id, function (data) {
            callback(data);
        });
    }

    var getUserComments = function (user_id, callback) {
        $.getJSON("http://localhost:5001/api/user/usercomments/" + user_id, function (data) {
            callback(data);
        });
    }

    return {
        searchedPosts,
        changePage,
        getQuestion,
        getAnswers,
        getFavorites,
        postFavorite,
        deleteFavorite,
        getAllFavorites,
        getNote,
        postNote,
        deleteNote,
        putNote,
        getHistory,
        getRelatedWords,
        getUser,
        getUserPosts,
        getUserComments,
        getTermNetwork, 
        refreshPostsPage,
        refreshFavorites,
        refreshHistory
    };

});