define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        // console.log("fra posts:", params);

        var posts = ko.observableArray([]);
        var prev = ko.string;
        var next = ko.string;
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);
        var userSearchString = ko.observable("");
        var searchingString = ko.observable("");
        var searchHasResults = ko.observable(true);
        var showSearch = ko.observable(false);

        // ------------ Search Function: ------------ //
        var search = function () {
            dataservice.searchedPosts(userSearchString(),
                data => {
                    if (data.data[0].body === "No search result") {
                        console.log("No search result from dataservice");
                        searchHasResults(false);
                        searchingString(data.data[0].body);
                    } else {
                        console.log("data fra search-func:", data);
                        posts.removeAll();
                        for (i = 0; i < data.data.length; i++) {
                            posts.push(data.data[i]);
                        }
                        next = data.next;
                        prev = data.prev;
                        navPage();
                        currentState = data;
                        searchHasResults(true);
                        searchingString('Search result of "' + userSearchString() + '"');
                    }
                });
            showSearch(true);
            bc.publish(bc.events.changeData, { search_string: userSearchString() });
        }

        // ------------ Page Navigation: ------------ //
        var navPage = function (data) {
            next === null ? displayNext(false) : displayNext(true);
            prev === null ? displayPrev(false) : displayPrev(true);
            $('html,body').animate({ scrollTop: 120 }, 300);
        }

        var nextPage = function () {
            console.log("pressed next");
            dataservice.changePage(next, data => {
                posts.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    posts.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        var prevPage = function () {
            console.log("pressed prev");
            dataservice.changePage(prev, data => {
                posts.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    posts.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        var currentState = {};

        // ------------ Get individual post: ------------ //
        var getPost = function () {
            bc.publish(bc.events.changeView, { name: "single-post", data: this, state: currentState });
        }

        // ------------ Control state: ------------ //
        // console.log("params fra posts;", params);
        
        if (params.fp_msg || params.fp_msg === "")
        {
            userSearchString(params.fp_msg);
            search();
        } 
        else if (params.nav_msg || params.nav_msg === "") {
            console.log(params.nav_msg);
            userSearchString(params.nav_msg);
            search();
        }
        else if (!jQuery.isEmptyObject(params))
        {
            posts.removeAll();
            for (i = 0; i < params.data.length; i++) {
                posts.push(params.data[i]);
            }
            next = null;
            prev = null;
            navPage();
            currentState = params;
            searchHasResults(true);
            searchingString('Search result of "' + userSearchString() + '"');
        }

        return {
            posts,
            displayPrev,
            displayNext,
            search,
            nextPage,
            prevPage,
            navPage,
            getPost,
            currentState,
            userSearchString,
            searchingString,
            searchHasResults,
            showSearch
        };

    }
});