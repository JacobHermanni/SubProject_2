define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        // console.log("params fra singlepost:", params);

        var postTitle = ko.observable();
        var creationDate = ko.observable();
        var score = ko.observable();
        var body = ko.observable();
        var user_id = ko.observable();
        var user_display_name = ko.observable();
        var comments = ko.observable();
        var answers = ko.observable();


        var getQuestion = (function () {
            dataservice.getQuestion(params.url, data => {
                postTitle(data.title);
                creationDate(data.creation_date);
                user_display_name(data.user_display_name);
                postTitle(data.question.title);
                user_id(data.user_id);
                score(data.score);
                body(data.body);
                comments(data.comments);
                answers(data.question.answers);

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
            comments,
            answers,
            postTitle,
            creationDate,
            score,
            body,
            user_id,
            user_display_name,
            back
        };

    }
});