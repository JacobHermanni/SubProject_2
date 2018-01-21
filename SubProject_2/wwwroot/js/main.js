require.config({
    baseUrl: "js",
    paths: {
        jquery: "lib/jQuery/dist/jquery.min",
        "knockout": "lib/knockout/dist/knockout",
        "text": "lib/text/text",
        "broadcaster": "services/broadcaster",
        "dataservice": "services/dataservice",
        "bootstrap": "lib/bootstrap/dist/js/bootstrap.min",
        jqcloud: "lib/jqcloud2/dist/jqcloud.min"
    },
    shim: {
        jqcloud: {
            deps: ["jquery"]
        },
        bootstrap: {
            deps: ["jquery"]
        }
    }
});

require(['knockout', 'jquery', 'jqcloud'], function (ko, $) {
    ko.bindingHandlers.cloud = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            // Hvad der først sker når 'cloud' binding benyttes. Word subscriber på en opdateringsfunktion der står nedenfor.
            var words = allBindings.get('cloud').words;
            if (words && ko.isObservable(words)) {
                words.subscribe(function () {
                    $(element).jQCloud('update', ko.unwrap(words));
                });
            }
        },
        // opdateringsfunktionen, der unwrapper observable arrayet words, så JQCloud kan læse dataen.
        update: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            // This will be called once when the binding is first applied to an element,
            // and again whenever any observables/computeds that are accessed change
            // Update the DOM element based on the supplied values here.
            var words = ko.unwrap(allBindings.get('cloud').words) || [];
            $(element).jQCloud(words);
        }
    };
});

// templates hvor både hmtl og js filer er wrapped i komponenter. Registreret genne ko.components så ko kan genkende dem per navnene. 
require(['knockout'], function (ko) {

    ko.components.register("all-posts", {
        viewModel: { require: "components/posts/posts" },
        template: { require: "text!components/posts/posts_view.html" }
    });

    ko.components.register("single-post", {
        viewModel: { require: "components/singlePost/single-post" },
        template: { require: "text!components/singlePost/single-post_view.html" }
    });

    ko.components.register("front-page", {
        viewModel: { require: "components/frontPage/front-page" },
        template: { require: "text!components/frontPage/front-page_view.html" }
    });

    ko.components.register("nav-bar-search", {
        viewModel: { require: "components/navBarSearch/nav-bar-search" },
        template: { require: "text!components/navBarSearch/nav-bar-search_view.html" }
    });

    ko.components.register("user-page", {
        viewModel: { require: "components/userPage/user-page" },
        template: { require: "text!components/userPage/user-page_view.html" }
    });

    ko.components.register("favorites-page", {
        viewModel: { require: "components/favoritesPage/favorites-page" },
        template: { require: "text!components/favoritesPage/favorites-page_view.html" }
    });

    ko.components.register("history-page", {
        viewModel: { require: "components/historyPage/history-page" },
        template: { require: "text!components/historyPage/history-page_view.html" }
    });

});


require(["knockout", "jquery", "broadcaster", "jqcloud", "bootstrap"], function (ko, jQuery, broadcaster, bootstrap) {
    (function () {

        var vm = (function () {

            var currentView = ko.observable('front-page');
            var navSearch = ko.observable('nav-bar-search');
            var currentParams = ko.observable(null);

            // obsolete
            var switchComponent = function () {
                if (currentView() === "all-posts") {
                    currentView("single-post");
                } else {
                    currentView("all-posts");
                }
            }

            var favoritesState = {};
            var allPostsState = {};
            var historyState = {};


            // gem states fra de undersider, der har pagination og variabler som søgefraser, der skal gemmes til næste besøg af undersiden.
            broadcaster.subscribe(broadcaster.events.updateState,
                updateInfo => {
                    console.log("updating state from", updateInfo.from);

                    switch (updateInfo.from) {
                        case "all-posts":
                            if (updateInfo.url !== undefined) {
                                console.log("coming from posts", updateInfo.url);
                                allPostsState = { searchString: updateInfo.searchString, url: updateInfo.url };
                            }
                            break;

                        case "favorites-page":
                            console.log("coming from favorites", updateInfo.url);
                            favoritesState = updateInfo.url;
                            break;

                        case "history-page":
                            console.log("coming from history", updateInfo.url);
                            historyState = { url: updateInfo.url };
                            allPostsState = { search: updateInfo.search };
                            break;

                        default:
                            break;
                    }
                });


            broadcaster.subscribe(broadcaster.events.changeView,
                viewInfo => {

                    // hvor kan et undersideskift komme fra og gem state omkring search frase og pagination
                    switch (viewInfo.from) {

                        //obsolete

                        //case "all-posts":
                        //    if (viewInfo.selfUrl !== undefined) {
                        //        allPostsState = { searchString: viewInfo.searchString, url: viewInfo.selfUrl };
                        //    }
                        //    break;

                        //case "favorites-page":
                        //    console.log("coming from favorites", viewInfo.selfUrl);
                        //    favoritesState = viewInfo.selfUrl;
                        //    break;

                        //case "single-page":
                        //    console.log("coming from single-post", viewInfo.id);
                        //    break;

                        case "history-page":
                            console.log("coming from history", viewInfo.url);
                            historyState = { url: viewInfo.url };
                            allPostsState = { search: viewInfo.search };
                            break;

                        case "nav-search":
                            console.log("coming from navbar", viewInfo.search);
                            allPostsState = { search: viewInfo.search };
                            break;

                        case "front-page":
                            console.log("coming from frontpage", viewInfo.search);
                            allPostsState = { search: viewInfo.search };
                            break;


                        default:
                            break;
                    }

                    switch (viewInfo.to) {
                        case "all-posts":
                            currentParams(allPostsState);
                            break;

                        case "single-post":
                            currentParams({ url: viewInfo.singlePostUrl, from: viewInfo.from });
                            break;

                        case "favorites-page":
                            currentParams(favoritesState);
                            break;

                        case "history-page":
                            currentParams(historyState);
                            break;

                        case "user-page":
                            currentParams(viewInfo.id);
                            break;

                        default:
                            break;
                    }
                    
                    // opdater currentview
                    currentView(viewInfo.to);
                });


            return {
                currentView,
                switchComponent,
                currentParams,
                navSearch
                // obsolete da de ikke er data-bindings
                //favoritesState,
                //allPostsState,
                //historyState
            }

        })();


        ko.applyBindings(vm);


    })();
})