define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        // --- SKAL ÆNDRES DA DETTE BLOT ER FRA TEST:: --- //
        var userID = params.fp_msg;

        console.log(params.fp_msg);

        var user_id = ko.observable();
        var user_name = ko.observable();
        var user_creation_date = ko.observable();
        var user_age = ko.observable();
        var user_location = ko.observable();

        var user_posts = ko.observableArray([]);
        var totalNumberOfPosts = ko.observable();
        var numberOfPagesOfPosts = ko.observable();

        var user_comments = ko.observableArray([]);
        var totalNumberOfComments = ko.observable();
        var numberOfPagesOfComments = ko.observable();

        var displayPosts = ko.observable(false);
        var displayComments = ko.observable(false);

        // ------------ Find favorites self-invoking function: ------------ //
        var findUserInfo = (function () {
            dataservice.getUser(userID, data => {
                user_id(data.user.user_id);
                user_name(data.user.user_display_name);
                user_creation_date(data.user.user_creation_date);
                user_age(data.user.user_age);
                user_location(data.user.user_location);
            });
        })();

        var postBool = false;

        var findUserPosts = function () {
            dataservice.getUserPosts(userID, data => {
                user_posts.removeAll();
                for (i = 0; i < data.userPosts.length; i++) {
                    user_posts.push(data.userPosts[i]);
                }
                totalNumberOfPosts(data.total);
                numberOfPagesOfPosts(data.pages);

                if (postBool == false) {                     postBool = true;                     displayPosts(true);                 } else {                     postBool = false;                     displayPosts(false);                 }
            });
        }

        var commentsBool = false;

        var findUserComments = function () {
            dataservice.getUserComments(userID, data => {
                user_comments.removeAll();
                for (i = 0; i < data.userComments.length; i++) {
                    user_comments.push(data.userComments[i]);
                }
                totalNumberOfComments(data.total);
                numberOfPagesOfComments(data.pages);

                if (commentsBool == false) {
                    commentsBool = true;
                    displayComments(true);
                } else {
                    commentsBool = false;
                    displayComments(false);
                }
            });
        }

        return {
            user_id,
            user_name,
            user_creation_date,
            user_age,
            user_location,
            findUserPosts,
            displayPosts,
            displayComments,
            user_posts,
            totalNumberOfPosts,
            numberOfPagesOfPosts,
            findUserComments,
            user_comments,
            totalNumberOfComments,
            numberOfPagesOfComments
        };

    }
});