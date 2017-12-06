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
            .fail(function(jqXHR, textStatus, errorThrown)
            {
                console.log('getJSON request failed! ' + textStatus);
            })
            .always(function()
            {
                console.log('getJSON request ended!'); 
            });
    }

    return {
        searchedPosts,
    };

});