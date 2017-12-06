define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        // console.log("params fra singlepost:", params);

        var postTitle = ko.observable();
        var creationDate = ko.observable();
        var score = ko.observable();
        var body = ko.observable();

        var answers = ko.observableArray([]);

        var getQuestion = (function () {
            dataservice.getQuestion(params.url, data => {
                postTitle(data.title);
                creationDate(data.creationDate);
                score(data.score);
                body(data.body);
                getAnswers(data.answers);
            })
        })();

        var getAnswers = function (url) {
            dataservice.getAnswers(url, data => {
                for (i = 0; i < data.length; i++) {
                    answers.push(data[i]);
                    //console.log(data[i]);
                }
            });
        }

        var back = function() {
            bc.publish(bc.events.changeView, { name: "all-posts" } );
        }

        return {
            getQuestion,
            getAnswers,
            answers,
            postTitle,
            creationDate,
            score,
            body,
            back
        };

    }
});