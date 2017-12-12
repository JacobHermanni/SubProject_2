define(['knockout', 'broadcaster', 'dataservice', 'bootstrap'], function (ko, bc, dataservice, bootstrap) {
    return function (params) {


        var favorites = ko.observableArray([]);
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);
        var prev = ko.string;
        var next = ko.string;

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


        var getPost = function () {
            bc.publish(bc.events.changeView, { name: "single-post", data: this });
        }


        // ------------ Favorite Removal functionality: ------------ //
        
        var removeFromFavorites = function () {
            dataservice.deleteFavorite(tempFavId, data => {
                findFavorites();
            });

        }


        // ------------ Note functionality: ------------ //

        var tempFavId;

        var resetNewNote = function() {
            newNoteBody("");
            displayNewSave(false);
        }

        var visibleOptions = false;

        var setOptionsFalse = function () {
            visibleOptions = false;
            displayOptions(false);
        }

        var showOptions = function () {
            if (visibleOptions == false) {
                visibleOptions = true;
                displayOptions(true);
            } else {
                visibleOptions = false;
                displayOptions(false);
            }
        }

        var getFavId = function(favorite) {
            tempFavId = favorite.favorite_id;
            console.log("tempFavID:", tempFavId);
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
            dataservice.putNote(tempFavId, newNoteBody(), data => {
                findFavorites();              
            });
            resetNewNote();
            setOptionsFalse();
            displayNormalSave(true);
        }

        var createNote = function() {
            dataservice.postNote(tempFavId, newNoteBody(), data => {
                findFavorites();              
            });
            resetNewNote();
        }

        var deleteNote = function () {
            dataservice.deleteNote(tempFavId, data => {
                findFavorites();              
            });
            setOptionsFalse();
        }

        // Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
        // Could be stored in a separate utility library - snatched directly from: http://knockoutjs.com/examples/animatedTransitions.html
        ko.bindingHandlers.fadeVisible = {
            init: function(element, valueAccessor) {
                // Initially set the element to be instantly visible/hidden depending on the value
                var value = valueAccessor();
                $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
            },
            update: function(element, valueAccessor) {
                // Whenever the value subsequently changes, slowly fade the element in or out
                var value = valueAccessor();
                ko.unwrap(value) ? $(element).fadeIn() : $(element).fadeOut();
            }
        };


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
            displayOptions,
            setOptionsFalse,
            removeFromFavorites,
            next,
            prev
        };

    }
});