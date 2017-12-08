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
        var searchVisible = ko.observable(false);
        var searchHasResults = ko.observable(false);


        // ------------ Search Function: ------------ //
        var search = function () {
            dataservice.searchedPosts(userSearchString(),
                data => {
                    if (data === "error") {
                        console.log("error");
                        searchHasResults(false);
                        this.searchingString('No search result');
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
                        this.searchingString('Search result of "' + userSearchString() + '"');
                    }
                });
            bc.publish(bc.events.changeData, { search_string: userSearchString() });
        }

        var searchFromFrontPageOrNav = function (searchString) {
            userSearchString(searchString);
            dataservice.searchedPosts(searchString, data => {
                // console.log("data fra navOrFront-search-func:", data);
                posts.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    posts.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
                currentState = data;
            });
        }

        // ------------ Page Navigation: ------------ //
        var navPage = function (data) {
            next === null ? displayNext(false) : displayNext(true);
            prev === null ? displayPrev(false) : displayPrev(true);
            $('html,body').animate({ scrollTop: 120 }, 300);
            searchVisible(true);
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
            searchFromFrontPageOrNav(params.fp_msg);
        } 
        else if (params.nav_msg || params.nav_msg === "")
        {
            searchFromFrontPageOrNav(params.nav_msg);
        }
        else if (params != null)
        {
            posts.removeAll();
            for (i = 0; i < params.data.length; i++) {
                posts.push(params.data[i]);
            }
            next = null;
            prev = null;
            navPage();
            currentState = params;
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
            searchFromFrontPageOrNav,
            searchVisible,
            searchingString,
            searchHasResults
        };

    }
});