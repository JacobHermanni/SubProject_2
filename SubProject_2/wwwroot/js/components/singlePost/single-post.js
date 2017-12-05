define(['knockout', 'broadcaster'], function (ko, bc) {
    return function (params) {

        console.log("params fra singlepost:", params);

        fetchData = function (url, callback) {
            $.getJSON(url, function (data) {
                //console.log("fetched Data single-post:", data);
                callback(data);

            });
        }

        var postTitle = ko.observable();
        var creationDate = ko.observable();
        var score = ko.observable();
        var body = ko.observable();

        var answers = ko.observableArray([]);

        var getQuestion = function (url) {
            fetchData(url, data => {
                postTitle(data.title);
                creationDate(data.creationDate);
                score(data.score);
                body(data.body);
                getAnswers(data.answers);
            })
        }

        getQuestion(params.link);

        var getAnswers = function (url) {
            //console.log("Answers", url);
            fetchData(url, data => {
                //console.log("fra getANSWERS:::", data);
                for (i = 0; i < data.length; i++) {
                    answers.push(data[i]);
                    //console.log(data[i]);
                }
            });
        }

        var back = function() {
            bc.publish(bc.events.changeView, { name: "all-posts"});
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