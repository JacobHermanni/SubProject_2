﻿define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var histories = ko.observableArray([]);
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);
        var historyID = ko.observable();
        var searchString = ko.observable();
        var historyTimestamp = ko.observable();
        var historyUrl = ko.observable();


        var getHistory = (function () {
        dataservice.getQuestion(params.url, data => {

            historyID(data.history_id);
            searchString(data.search_string);
            historyTimestamp(data.history_timestamp);
            historyUrl(data.url);

            })
        })();

        // ------------ Find favorites self-invoking function: ------------ //
        var findSearchHistory = (function () {
            dataservice.getHistory( data => {
                histories.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    histories.push(data.data[i]);
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
                histories.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    histories.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }

        var prevPage = function () {
            console.log("pressed prev");
            dataservice.changePage(prev, data => {
                histories.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    histories.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                navPage();
            });
        }


        return {
            histories,
            nextPage,
            prevPage,
            displayNext,
            displayPrev,
            historyID,
            searchString,
            historyTimestamp,
            historyUrl

        };

    }
});