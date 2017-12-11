require.config({
    baseUrl: "js",
    paths: {
        "jQuery": "lib/jQuery/dist/jquery.min",
        "knockout": "lib/knockout/dist/knockout",
        "text": "lib/text/text",
        "broadcaster": "services/broadcaster",
        "dataservice": "services/dataservice",
        "bootstrap": "lib/bootstrap/dist/js/bootstrap.min"
    }
});

function test() {
    console.log("works");
}

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


require(["knockout", "jQuery", "broadcaster"], function (ko, jQuery, broadcaster) {
    (function () {

        var vm = (function () {

            var currentView = ko.observable('front-page');
            var navSearch = ko.observable('nav-bar-search');
            var currentParams = ko.observable(null);

            var switchComponent = function () {
                if (currentView() === "all-posts") {
                    currentView("single-post");
                } else {
                    currentView("all-posts");
                }
            }

            var currentState = {};

            broadcaster.subscribe(broadcaster.events.changeView,
                viewInfo => {
                    // console.log("viewinfo from main", viewInfo);
                    currentView(viewInfo.name);

                    // if there is no data, it means single-post is switching view to all-posts and state is relevant. Else the data is for single-post.
                    if (viewInfo.data !== undefined) {
                        console.log("changing state info in main");
                        currentParams(viewInfo.data);
                        currentState = viewInfo.state;
                        // console.log("currentState", currentState);
                    } else {
                        currentParams(currentState);
                    }

                    if (viewInfo.fp_msg || viewInfo.fp_msg === "") {
                        console.log("coming from fp_msg", viewInfo.fp_msg);
                        currentParams({ fp_msg: viewInfo.fp_msg });
                    }

                    if (viewInfo.nav_msg || viewInfo.nav_msg === "") {
                        console.log("coming from nav_msg", viewInfo.nav_msg);
                        currentParams({ nav_msg: viewInfo.nav_msg });
                    }

                    if (viewInfo === null || viewInfo === undefined) {
                        currentParams("");
                    }
                });

            return {
                currentView,
                switchComponent,
                currentParams,
                currentState,
                navSearch,
            }

        })();


        ko.applyBindings(vm);


    })();
})