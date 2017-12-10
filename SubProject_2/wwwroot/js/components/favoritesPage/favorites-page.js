define(['knockout', 'broadcaster', 'dataservice', 'bootstrap'], function (ko, bc, dataservice, bootstrap) {
    return function (params) {


        var favorites = ko.observableArray([]);
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);

        var noteBody = ko.observable();
        var noteTime = ko.observable();

        var newNoteBody = ko.observable("");

        // ------------ Find favorites function: ------------ //
        var findFavorites = function () {
            dataservice.getFavorites( data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        };

        findFavorites();

        // ------------ Page Navigation: ------------ //
        var navPage = function (data) {
            next === null || undefined ? displayNext(false) : displayNext(true);
            prev === null || undefined ? displayPrev(false) : displayPrev(true);
        }

        var nextPage = function () {
            console.log("pressed next");
            dataservice.changePage(next, data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        var prevPage = function () {
            console.log("pressed prev");
            dataservice.changePage(prev, data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        // ------------ Get individual post: ------------ //
        var getPost = function () {
            bc.publish(bc.events.changeView, { name: "single-post", data: this });
        }

        var getNote = function (favorite) {
            dataservice.getNote(favorite.favorite_id, data => {
                 noteBody(data.body);
                 noteTime(data.created_timestamp);
            });
        }

        var editNote = function () {
            console.log("ok nu skal der altså ske noget");
        }

        var tempFavId;

        var getFavId = function(favorite) {
            tempFavId = favorite.favorite_id;
            console.log("tempFavID:", tempFavId);
        }

        var createNote = function() {
            // console.log("we are going to create a note");
            // console.log("wiv this bod:", newNoteBody());
            // console.log("wiv this fav_id:", tempFavId);

            dataservice.postNote(tempFavId, newNoteBody(), data => {
                
            });
            findFavorites();
        }

        return {
            favorites,
            nextPage,
            prevPage,
            displayNext,
            displayPrev,
            getPost,
            getNote,
            noteBody,
            noteTime,
            editNote,
            createNote,
            newNoteBody,
            getFavId
        };

    }
});