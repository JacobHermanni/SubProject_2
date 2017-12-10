define(['knockout', 'broadcaster', 'dataservice', 'bootstrap'], function (ko, bc, dataservice, bootstrap) {
    return function (params) {


        var favorites = ko.observableArray([]);
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);

        var noteBody = ko.observable();
        var noteTime = ko.observable();

        var newNoteBody = ko.observable("");
        var displayNewSave = ko.observable(false);
        var displayNormalSave = ko.observable(true);

        var displayOptions = ko.observable(false);

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
        var tempFavId;

        var resetNewNote = function() {
            newNoteBody("");
            displayNewSave(false);
        }

        var bool = false;
        var showOptions = function () {
            if (bool == false) {
                bool = true;
                displayOptions(true);
            } else {
                bool = false;
                displayOptions(false);
            }
        }

        var getFavId = function(favorite) {
            tempFavId = favorite.favorite_id;
            console.log("tempFavID:", tempFavId);
        }

        var getPost = function () {
            bc.publish(bc.events.changeView, { name: "single-post", data: this });
        }

        var getNote = function (favorite) {
            dataservice.getNote(favorite.favorite_id, data => {
                 noteBody(data.body);
                 noteTime(data.created_timestamp);
            });
            getFavId(favorite);
        }

        var editNote = function () {
            displayNewSave(true);
            displayNormalSave(false);
            dataservice.getNote(tempFavId, data => {
                 newNoteBody(data.body);
            });
        }

        var updateNote = function () {
            dataservice.putNote(tempFavId, newNoteBody());
            resetNewNote();
        }

        var createNote = function() {
            dataservice.postNote(tempFavId, newNoteBody());
            findFavorites();
        }

        var deleteNote = function () {
            dataservice.deleteNote(tempFavId);
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
            getFavId,
            deleteNote,
            updateNote,
            resetNewNote,
            displayNewSave,
            displayNormalSave,
            showOptions,
            displayOptions
        };

    }
});