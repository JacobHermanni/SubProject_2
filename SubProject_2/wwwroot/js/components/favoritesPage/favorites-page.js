define(['knockout', 'broadcaster', 'dataservice', 'bootstrap'], function (ko, bc, dataservice, bootstrap) {
    return function (params) {


        var favorites = ko.observableArray([]);
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);

        var noteBody = ko.observable();
        var noteTime = ko.observable();

        // ------------ Find favorites self-invoking function: ------------ //
        var findFavorites = (function () {
            dataservice.getFavorites( data => {
                favorites.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    favorites.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        })();

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

        return {
            favorites,
            nextPage,
            prevPage,
            displayNext,
            displayPrev,
            getPost,
            getNote,
            noteBody,
            noteTime
        };

    }
});