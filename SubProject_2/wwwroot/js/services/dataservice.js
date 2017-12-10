﻿define([], function() {

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
                    data: [ {body: "No search result"} ],
                    next: null,
                    prev: null
                };
                console.log('Failed search returns object: ', furb);
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

    var getNote = function (fav_id, callback) {
        $.getJSON("http://localhost:5001/api/favorite/note/" + fav_id, function (data) {
            console.log("got note:", data);
            callback(data);
        });
    }

    var postNote = function (fav_id, noteString, callback) {
        
        var jsonData = JSON.stringify({favorite_id: fav_id, body: noteString});

        $.ajax("http://localhost:5001/api/favorite/note/", {
            data : jsonData,
            contentType : 'application/json',
            type : 'POST',
            success: function(){
                console.log("succesfully created a note");
            }
        });

    }

    return {
        searchedPosts,
        changePage,
        getQuestion,
        getAnswers,
        getFavorites,
        getHistory,
        getNote,
        postNote
    };

});