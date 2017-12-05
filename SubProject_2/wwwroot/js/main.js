require.config({
    baseUrl: "js",
    paths: {
        "jQuery": "lib/jQuery/dist/jquery.min",
        "knockout": "lib/knockout/dist/knockout",
        "text": "lib/text/text",
        "broadcaster": "services/broadcaster"
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


});


require(["knockout", "jQuery", "broadcaster"], function (ko, jQuery, broadcaster) {
    (function () {

        fetchData = function (url, callback) {
            $.getJSON(url, function (data) {
                console.log("fetched Data:", data);
                callback(data);
            });
        }

        var vm = (function () {

            var currentView = ko.observable('all-posts');
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
                    console.log("viewinfo from main", viewInfo);
                    currentView(viewInfo.name);

                    // if there is no data, it means single-post is switching view to all-posts and state is relevant. Else the data is for single-post.
                    if (viewInfo.data !== undefined) {
                        currentParams(viewInfo.data);
                        currentState = viewInfo.state;
                    } else {
                        currentParams(currentState);
                    }
                });

            return {
                currentView,
                switchComponent,
                currentParams,
                currentState
            }

        })();


        ko.applyBindings(vm);


    })();
})