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
        var currentPage = ko.observable();
        var totalPages = ko.observable();
        var totalFavorites = ko.observable();
        var selfUrl;



        // ------------ Find favorites function: ------------ //
        var findFavorites = function () {
            dataservice.getFavorites(data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                totalPages(data.pages);
                totalFavorites(data.total);
                selfUrl = data.url;
                navPage();
                bc.publish(bc.events.updateState, { from: "favorites-page", url: selfUrl });
            });
        };

        // opdaterer på den side brugeren er/var på sidst, så pagination fungerer.
        var refresh = function(url) {
            dataservice.refreshFavorites(url,
                data => {
                    favorites.removeAll();
                    for (i = 0; i < data.data.length; i++) {
                        favorites.push(data.data[i]);
                    }
                    next = data.next;
                    prev = data.prev;
                    currentPage((data.page) + 1);
                    totalPages(data.pages);
                    totalFavorites(data.total);
                    selfUrl = data.url;
                    navPage();
                    bc.publish(bc.events.updateState, { from: "favorites-page", url: selfUrl });
                });
        }


        // ------------ Page Navigation: ------------ //
        var navPage = function (data) {
            next === null || undefined ? displayNext(false) : displayNext(true);
            prev === null || undefined ? displayPrev(false) : displayPrev(true);
        }

        // næste side og opdater state i main.js
        var nextPage = function () {
            // next er url'en til næste side
            dataservice.changePage(next, data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                selfUrl = data.url;
                navPage();
                bc.publish(bc.events.updateState, { from: "favorites-page", url: selfUrl });
            });
        }

        // forrige side og opdater state i main.js
        var prevPage = function () {
            // prev er url'en til forrige side
            dataservice.changePage(prev, data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                selfUrl = data.url;
                navPage();
                bc.publish(bc.events.updateState, { from: "favorites-page", url: selfUrl });
            });
        }

        // Check params to recreate same page as user left it
        if (params !== undefined) {
            if (!jQuery.isEmptyObject(params)) {
                refresh(params);
            } else {
                findFavorites();
            }
        }


        // ------------ Get individual post: ------------ //


        var getPost = function () {
            bc.publish(bc.events.changeView, { to: "single-post", from: "favorites-page", singlePostUrl: this.url, url: selfUrl });
        }


        // ------------ Favorite Removal functionality: ------------ //

        var removeFromFavorites = function () {
            dataservice.deleteFavorite(tempFavId, data => {
                // refresh siden bagefter for at have den rette liste af favorites
                refresh(selfUrl);
            });

        }


        // ------------ Note functionality: ------------ //

        // temp favId er for at vise det berørte listitem. Benyttes under redigering af noter og sletning af favorit,
        // da en modal skal bekræfte ændringerne og disse modals ikke har selvreferencer til det berørte listitem.
        var tempFavId;

        var resetNewNote = function () {
            newNoteBody("");
            // save knap for at gemme en opdatering på en eksisterende note (save (update))
            displayNewSave(false);
        }

        var visibleOptions = false;

        var setOptionsFalse = function () {
            visibleOptions = false;
            displayOptions(false);
        }

        // nærmere en 'toggle' options
        var showOptions = function () {
            if (visibleOptions == false) {
                visibleOptions = true;
                // observable der bestemmer om tandhjulet er synligt
                displayOptions(true);
            } else {
                // toggle af hvis den var synlig
                visibleOptions = false;
                displayOptions(false);
            }
        }

        // temp favId er for at vise det berørte listitem. Benyttes under redigering af noter og sletning af favorit,
        // da en modal skal bekræfte ændringerne og disse modals ikke har selvreferencer til det berørte listitem.
        var getFavId = function (favorite) {
            tempFavId = favorite.favorite_id;
        }

        var getNote = function (favorite) {
            dataservice.getNote(favorite.favorite_id, data => {
                noteBody(data.body);
                noteTime(data.created_timestamp);
            });
            getFavId(favorite);
        }

        var editNote = function () {
            // save(update) synlig
            displayNewSave(true);
            // skjul save til ny note
            displayNormalSave(false);
            // sæt redigerings body til eksisterende note, så noten kan redigeres ud fra det allerede eksisterende indhold.
            dataservice.getNote(tempFavId, data => {
                newNoteBody(data.body);
            });
        }

        var updateNote = function () {
            dataservice.putNote(tempFavId, newNoteBody(), data => {
                // refresh siden bagefter for at have den rette liste af favorites
                refresh(selfUrl);
            });
            // reset modal og options knap + tom tekstboks
            resetNewNote();
            setOptionsFalse();
            displayNormalSave(true);
        }

        var createNote = function () {
            // tjek for om noten er for kort
            if (newNoteBody().length < 1) {
                alert("A note that long has no real value does it?");
            } else {
                dataservice.postNote(tempFavId, newNoteBody(), data => {
                    // refresh siden bagefter for at have den rette liste af favorites
                    refresh(selfUrl);
                    resetNewNote();
                });
            }
        }

        var deleteNote = function () {
            dataservice.deleteNote(tempFavId, data => {
                // refresh siden bagefter for at have den rette liste af favorites
                refresh(selfUrl);
            });
            setOptionsFalse();
        }

        // Here's a custom Knockout binding that makes elements shown/hidden via jQuery's fadeIn()/fadeOut() methods
        // Could be stored in a separate utility library - snatched directly from: http://knockoutjs.com/examples/animatedTransitions.html
        ko.bindingHandlers.fadeVisible = {
            init: function (element, valueAccessor) {
                // Initially set the element to be instantly visible/hidden depending on the value
                var value = valueAccessor();
                $(element).toggle(ko.unwrap(value)); // Use "unwrapObservable" so we can handle values that may or may not be observable
            },
            update: function (element, valueAccessor) {
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
            prev,
            currentPage,
            totalPages,
            totalFavorites,
            selfUrl
        };

    }
});