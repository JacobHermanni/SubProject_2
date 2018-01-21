define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {


        var userID = params;

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
                user_creation_date(data.user.formatted_date);
                user_age(data.user.user_age);
                user_location(data.user.user_location);
            });
        })();


        // ------------------------ USER POSTS: ------------------------ //

        var postBool = false;
        var nextP;
        var prevP;
        var dnP = ko.observable(false);
        var dpP = ko.observable(false);

        var findUserPosts = function () {
            dataservice.getUserPosts(userID, data => {
                user_posts.removeAll();
                for (i = 0; i < data.userPosts.length; i++) {
                    user_posts.push(data.userPosts[i]);
                }
                totalNumberOfPosts(data.total);
                numberOfPagesOfPosts(data.pages);
                nextP = data.next;
                prevP = data.prev;
                navPageP();

                // toggle synlighed af bruger-posts
                if (postBool == false) {                     postBool = true;                     displayPosts(true);                 } else {                     postBool = false;                     displayPosts(false);                 }
            });
        }

        // ------------ Page Navigation: ------------ //
        var navPageP = function (data) {
            nextP === null || undefined ? dnP(false) : dnP(true);
            prevP === null || undefined ? dpP(false) : dpP(true);
        }

        var nextPageP = function () {
            dataservice.changePage(nextP, data => {
                user_posts.removeAll();
                for (i = 0; i < data.userPosts.length; i++) {
                    user_posts.push(data.userPosts[i]);
                }
                nextP = data.next;
                prevP = data.prev;
                navPageP();
            });
        }

        var prevPageP = function () {
            dataservice.changePage(prevP, data => {
                user_posts.removeAll();
                for (i = 0; i < data.userPosts.length; i++) {
                    user_posts.push(data.userPosts[i]);
                }
                nextP = data.next;
                prevP = data.prev;
                navPageP();
            });
        }


        // ------------------------ USER COMMENTS: ------------------------ //

        var commentsBool = false;
        var nextC;
        var prevC;
        var dnC = ko.observable(false);
        var dpC = ko.observable(false);

        var findUserComments = function () {
            dataservice.getUserComments(userID, data => {
                user_comments.removeAll();
                for (i = 0; i < data.userComments.length; i++) {
                    user_comments.push(data.userComments[i]);
                }
                totalNumberOfComments(data.total);
                numberOfPagesOfComments(data.pages);
                nextC = data.next;
                prevC = data.prev;
                navPageC();

                if (commentsBool == false) {
                    commentsBool = true;
                    displayComments(true);
                } else {
                    commentsBool = false;
                    displayComments(false);
                }
            });
        }

        // ------------ Page Navigation: ------------ //
        var navPageC = function (data) {
            nextC === null || undefined ? dnC(false) : dnC(true);
            prevC === null || undefined ? dpC(false) : dpC(true);
        }

        var nextPageC = function () {
            dataservice.changePage(nextC, data => {
                user_comments.removeAll();
                for (i = 0; i < data.userComments.length; i++) {
                    user_comments.push(data.userComments[i]);
                }
                nextC = data.next;
                prevC = data.prev;
                navPageC();
            });
        }

        var prevPageC = function () {
            dataservice.changePage(prevC, data => {
                user_comments.removeAll();
                for (i = 0; i < data.userComments.length; i++) {
                    user_comments.push(data.userComments[i]);
                }
                nextC = data.next;
                prevC = data.prev;
                navPageC();
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
            numberOfPagesOfComments,
            nextPageP,
            prevPageP,
            dnP,
            dpP,
            nextPageC,
            prevPageC,
            dnC,
            dpC
        };

    }
});