define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var postTitle = ko.observable();
        var creationDate = ko.observable();
        var score = ko.observable();
        var body = ko.observable();
        var user_id = ko.observable();
        var user_display_name = ko.observable();
        var comments = ko.observable();
        var answers = ko.observable();
        var test = ko.observable();


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
                answers(data.question);
                getAnswers(data.question.answersUrl);
            })
        })();


        var getAnswers = function (url) {
            dataservice.getAnswers(url, data => {
            test (data.answers);
            next = data.next;
            prev = data.prev;
            console.log("mit link", prev);
            });
        }

        var getNext = function () {
            dataservice.getAnswers(next, data => {
            test (data.answers);
            next = data.next;
            prev = data.prev;
            });
        }

        var getPrev = function () {
            dataservice.getAnswers(prev, data => {
            test (data.answers);
            prev = data.prev;
            next = data.next;
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
            test,
            getNext,
            getPrev,
            back

        };

    }
});