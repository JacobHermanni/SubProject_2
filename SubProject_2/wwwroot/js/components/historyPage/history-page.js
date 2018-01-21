define(['knockout', 'broadcaster', 'dataservice'], function (ko, bc, dataservice) {
    return function (params) {

        var histories = ko.observableArray([]);
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);
        var searchString = ko.observable();
        var historyTimestamp = ko.observable();
        var historyUrl = ko.observable();
        var selfUrl;
        var prev = ko.string;
        var next = ko.string;
        var currentPage = ko.observable();
        var totalPages = ko.observable();
        var totalHistory = ko.observable();


        // ------------ History search: ------------ //
        var fpSearchString = ko.observable();
        fpSearchString(searchString());

        var searched = function () {

            console.log(selfUrl);

            bc.publish(bc.events.changeView, { to: "all-posts", from: "history-page", search: this.search_string, url: selfUrl });
            bc.publish(bc.events.changeData, { search_string: fpSearchString() });

            $('html').animate({ scrollTop: 120 }, 300);
        }


        // ------------ Page Navigation: ------------ //
        var navPage = function (data) {
            next === null || undefined ? displayNext(false) : displayNext(true);
            prev === null || undefined ? displayPrev(false) : displayPrev(true);
        }

        var nextPage = function () {
            dataservice.changePage(next, data => {
                histories.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    histories.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                totalPages(data.pages);
                totalHistory(data.total);
                navPage();
                selfUrl = data.url;
                bc.publish(bc.events.updateState, { from: "history-page", url: selfUrl });
            });
        }

        var prevPage = function () {
            dataservice.changePage(prev, data => {
                histories.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    histories.push(data.data[i]);
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                totalPages(data.pages);
                totalHistory(data.total);
                navPage();
                selfUrl = data.url;
                bc.publish(bc.events.updateState, { from: "history-page", url: selfUrl });
            });
        }

        // Check params to recreate same page as user left it
        // genindlæs samme state hvis den har en. Ellers kald api på ny.
        if (params !== undefined) {
            if (!jQuery.isEmptyObject(params)) {
                dataservice.refreshHistory(params,
                    data => {
                        histories.removeAll();
                        for (i = 0; i < data.data.length; i++) {
                            histories.push(data.data[i]);
                        }
                        next = data.next;
                        prev = data.prev;
                        currentPage((data.page) + 1);
                        totalPages(data.pages);
                        totalHistory(data.total);
                        selfUrl = data.url;
                        navPage();
                    });
            } else {
                dataservice.getHistory(data => {
                    histories.removeAll();
                    for (i = 0; i < data.data.length; i++) {
                        histories.push(data.data[i]);
                    }
                    next = data.next;
                    prev = data.prev;
                    currentPage((data.page) + 1);
                    totalPages(data.pages);
                    totalHistory(data.total);
                    selfUrl = data.url;
                    navPage();
                });
            }
        }



        return {
            histories,
            nextPage,
            prevPage,
            displayNext,
            displayPrev,
            searchString,
            historyTimestamp,
            historyUrl,
            searched,
            fpSearchString,
            currentPage,
            totalPages,
            totalHistory,
            selfUrl
        };

    }
});