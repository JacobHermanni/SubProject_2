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
        var searchHasResults = ko.observable(false);
        var showSearch = ko.observable(false);
        var currentPage = ko.observable();
        var totalPages = ko.observable();
        var totalPosts = ko.observable();
        var showFavorite = ko.observable(false);

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
                        currentPage((data.page) +1);
                        totalPages(data.pages);
                        totalPosts(data.total);
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
                currentPage((data.page) + 1);
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
                currentPage((data.page) + 1);
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

        if (params.fp_msg || params.fp_msg === "") {
            userSearchString(params.fp_msg);
            search();
        }
        else if (params.nav_msg || params.nav_msg === "") {
            console.log(params.nav_msg);
            userSearchString(params.nav_msg);
            search();
        }
        else if (!jQuery.isEmptyObject(params)) {
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
            currentPage(params.data.page);
            totalPages(params.data.pages);
            totalPosts(params.data.total);
        }

        var favTest = function() {
            console.log("kommer favTest igennem??", this);
            var show = false;
            for (var i = 0; i < favorites.length; i++) {
                if (favorites[i].post_id == this.post_id) {
                    show = true;
                    break;
                }
            }
            return show;
        }

        var favorites;


        // Get favorites every pageload to check if results are on list
        var getFavorites = function () {
            dataservice.getFavorites(data => {
                console.log("data from favorites: ", data.data);
                favorites = data.data;
            });
        }();

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
            showSearch,
            currentPage,
            totalPages,
            totalPosts,
            favTest,
            getFavorites
        };

    }
});