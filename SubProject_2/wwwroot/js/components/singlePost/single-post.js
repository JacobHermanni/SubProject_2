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
        // changed name, should have been array. Still works!
        var answerposts = ko.observable();
        var getTags = ko.observable();
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);
        var next;
        var prev;


        var getQuestion = (function () {
            dataservice.getQuestion(params.url,
                data => {
                    postTitle(data.title);
                    creationDate(data.formatted_date);
                    user_display_name(data.user_display_name);
                    postTitle(data.question.title);
                    user_id(data.user_id);
                    score(data.score);
                    body(data.body);
                    comments(data.comments);
                    answers(data.question);
                    getAnswers(data.question.answersUrl);
                    getTags(data.question.tags);

                });
        })();

        var navPage = function (data) {
            next === null || undefined ? displayNext(false) : displayNext(true);
            prev === null || undefined ? displayPrev(false) : displayPrev(true);
            $('html,body').animate({ scrollTop: 120 }, 300);
        }




        var getAnswers = function (url) {
            dataservice.getAnswers(url, data => {
                answerposts(data.answers);
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        var getNext = function () {
            dataservice.getAnswers(next, data => {
                answerposts(data.answers);
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        var getPrev = function () {
            dataservice.getAnswers(prev, data => {
                answerposts(data.answers);
                prev = data.prev;
                next = data.next;
                navPage();
            });
        }

        var back = function () {
            bc.publish(bc.events.changeView, { to: params.from, from: "single-post" });
        }

        // ------------ Go to user: ------------ //
        var getUser = function () {
            var userID = user_id();
            bc.publish(bc.events.changeView, { to: "user-page", from: "single-post", id: userID });
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
            answerposts,
            getNext,
            getPrev,
            displayNext,
            displayPrev,
            back,
            getUser,
            getTags
        };

    }
});