define(['knockout', 'broadcaster', 'dataservice', 'jquery', 'bootstrap'], function (ko, bc, dataservice, $) {
    return function (params) {

        var posts = ko.observableArray([]);
        var prev = ko.string;
        var next = ko.string;
        var displayPrev = ko.observable(false);
        var displayNext = ko.observable(false);
        var userSearchString = ko.observable("");
        var searchingString = ko.observable("");
        var searchHasResults = ko.observable(false);
        var showSearch = ko.observable(false);
        var currentPage = ko.observable();
        var totalPages = ko.observable();
        var totalPosts = ko.observable();
        var favorites;

        var netWorkString = ko.observable();

        var words = ko.observableArray([]);

        var getRelatedWords = function () {
            words.removeAll();
            dataservice.getRelatedWords(userSearchString(),
                data => {
                    if (data !== undefined) {
                        for (i = 0; i < data.length - 1; i++) {
                            words.push({ text: data[i].term, weight: data[i].rank });
                        }
                    }
                });
        }

<<<<<<< HEAD
=======
        var termNetworkData;

        var getTermNetwork = function () {
            dataservice.getTermNetwork(userSearchString(), data => {
                termNetworkData = data;
            });
        }
>>>>>>> 86d883d... fjern alle console log clean style.css fix timestamps.

        var getFavorites = function () {
            dataservice.getAllFavorites(data => {
                favorites = data.data;
            });
        }

        // Get favorites every pageload to check if results are on list
        getFavorites();

        var DataItem = function (data) {
            this.mainData = data;
            this.favorite = ko.observable(checkForFavorite(data));
        }

        var currentState = {};

        // ------------ Search Function: ------------ //
        var search = function () {
            dataservice.searchedPosts(userSearchString(),
                data => {
                    if (data.data[0].body === "No search result") {
                        searchHasResults(false);
                        searchingString(data.data[0].body);
                    } else {
                        posts.removeAll();
                        for (i = 0; i < data.data.length; i++) {
                            posts.push(new DataItem(data.data[i]));
                        }
                        next = data.next;
                        prev = data.prev;
                        navPage();
                        searchingString('Search result of "' + userSearchString() + '"');
                        currentState = {
                            searchData: data,
                            posts: posts,
                            searchingString: searchingString(),
                            currentPage: data.page + 1,
                            totalPages: data.pages,
                            totalPosts: data.total,
                            userSearchString: userSearchString()
                        };
                        searchHasResults(true);
                        currentPage((data.page) + 1);
                        totalPages(data.pages);
                        totalPosts(data.total);
                    }
                });
            showSearch(true);
            bc.publish(bc.events.changeData, { search_string: userSearchString() });
        }

        // ------------ Page Navigation: ------------ //
        var navPage = function (data) {
            next === null ? displayNext(false) : displayNext(true);
            prev === null ? displayPrev(false) : displayPrev(true);
            $('html,body').animate({ scrollTop: 0 }, 300);
        }

        var nextPage = function () {
            dataservice.changePage(next, data => {
                posts.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    posts.push(new DataItem(data.data[i]));
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                currentState = {
                    searchData: data,
                    posts: posts,
                    searchingString: searchingString(),
                    currentPage: data.page + 1,
                    totalPages: data.pages,
                    totalPosts: data.total
                };
                navPage();
            });
        }

        var prevPage = function () {
            dataservice.changePage(prev, data => {
                posts.removeAll();
                for (i = 0; i < data.data.length; i++) {
                    posts.push(new DataItem(data.data[i]));
                }
                next = data.next;
                prev = data.prev;
                currentPage((data.page) + 1);
                currentState = {
                    searchData: data,
                    posts: posts,
                    searchingString: searchingString(),
                    currentPage: data.page + 1,
                    totalPages: data.pages,
                    totalPosts: data.total
                };
                navPage();
            });
        }

        // ------------ Get individual post: ------------ //
        var getPost = function () {
            bc.publish(bc.events.changeView, { name: "single-post", data: this.mainData, state: currentState });
        }


        // ------------ Control state: ------------ //

        if (params !== undefined) {
            if (params.fp_msg || params.fp_msg === "") {
                userSearchString(params.fp_msg);
                search();
            }
            else if (params.nav_msg || params.nav_msg === "") {
                userSearchString(params.nav_msg);
                search();
            }
            else if (!jQuery.isEmptyObject(params)) {
                posts.removeAll();
                posts = params.posts;
                next = params.searchData.next;
                prev = params.searchData.prev;
                navPage();
                currentPage(params.currentPage);
                totalPages(params.totalPages);
                totalPosts(params.totalPosts);
                currentState = params;
                searchHasResults(true);
                searchingString(params.searchingString);
                userSearchString(params.userSearchString);
                showSearch(true);
            }
        }


        var checkForFavorite = function (data) {
            var postId;
            if (data.parent_id) {
                postId = data.parent_id;
            } else {
                postId = data.post_id;
            }
            var show = false;
            for (var i = 0; i < favorites.length; i++) {
                if (favorites[i].post_id === postId) {
                    show = true;
                    break;
                }
            }
            return show;
        }

        var createFavorite = function (listObject) {
            var postId;
            if (listObject.mainData.parent_id) {
                postId = listObject.mainData.parent_id;
            } else {
                postId = listObject.mainData.post_id;
            }

            dataservice.postFavorite(postId, data => {
                getFavorites();
            });


            dataservice.searchedPosts(userSearchString(),
                data => {
                    posts.removeAll();
                    for (i = 0; i < data.data.length; i++) {
                        posts.push(new DataItem(data.data[i]));
                    }
                });
        }

        var deleteFavorite = function (listObject) {
            var postId;
            if (listObject.mainData.parent_id) {
                postId = listObject.mainData.parent_id;
            } else {
                postId = listObject.mainData.post_id;
            }

            for (var i = 0; i < favorites.length; i++) {
                if (favorites[i].post_id === postId) {
                    dataservice.deleteFavorite(favorites[i].favorite_id, data => {
                        getFavorites();
                    });
                    listObject.favorite(false);
                    break;
                }
            }
            dataservice.searchedPosts(userSearchString(),
                data => {
                    posts.removeAll();
                    for (i = 0; i < data.data.length; i++) {
                        posts.push(new DataItem(data.data[i]));
                    }
                });
        }

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


        var getTermNetwork = function () {
            dataservice.getTermNetwork(netWorkString(), data => {

                var nodes = [];
                var startNodesPos;
                var EndNodesPos;

                var links = [];
                var startLinkPos;
                var EndLinkPos;

                var sources = [];
                var targets = [];
                var values = [];


                for (i = 0; i < data.length; i++) {
                    if (data[i].termNetwork == "nodes:") {
                        startNodesPos = i+1;
                    }
                    if (data[i].termNetwork == "links:") {
                        EndNodesPos = i;
                        startLinkPos = i+1
                    }
                    if (data[i].termNetwork == "endOfLinks:") {
                        EndLinkPos = i;
                    }
                }

                for (i = startNodesPos; i < EndNodesPos; i++) {
                    nodes.push({name: data[i].termNetwork});
                }
                for (i = startLinkPos; i < EndLinkPos; i++) {
                    var sIdx = data[i].termNetwork.indexOf('s');
                    var tIdx = data[i].termNetwork.indexOf('t');
                    var vIdx = data[i].termNetwork.indexOf('v');

                    sources.push(parseInt(data[i].termNetwork.substring(sIdx+1, tIdx)));
                    targets.push(parseInt(data[i].termNetwork.substring(tIdx+1, vIdx)));
                    values.push(parseInt(data[i].termNetwork.substring(vIdx+1)));
                }

                var lengthOfLinks;
                lengthOfLinks = sources.length;

                for (i = 0; i < lengthOfLinks; i++) {
                    links.push({"source": sources[i], "target": targets[i], "value": values[i]});
                }

                nodesForGraph = nodes;
                linksForGraph = links;
            });


            var nodesForGraph = [];
            var linksForGraph = [];

            // JQeury removal of the existing graph so we don't get two.
            $( "svg" ).remove();

            setTimeout(function(){

                var graph =
                    {
                        "nodes": nodesForGraph,
                        "links": linksForGraph
                    }


                var width = 540,
                height = 500

                var svg = d3.select(".modal-body").append("svg")
                .attr("width", width)
                .attr("height", height);

                var force = d3.layout.force()
                .gravity(0.05)
                .distance(100)
                .charge(-200)
                .size([width, height]);

                //var color = d3.scaleOrdinal(d3.schemeCategory20);
                var color = d3.scale.category20c();

                //d3.json("graph.json", function(error, json) {
                    (function () {
                  //if (error) throw error;

                  force
                  .nodes(graph.nodes)
                  .links(graph.links)
                  .start();

                  var link = svg.selectAll(".link")
                  .data(graph.links)
                  .enter().append("line")
                  .attr("class", "link")
                  .attr("stroke-width", function(d) { return Math.sqrt(d.value); });

                  var node = svg.selectAll(".node")
                  .data(graph.nodes)
                  .enter().append("g")
                  .attr("class", "node")
                  .call(force.drag);
                  node.append("circle")
                  .attr("r", function(d) { return 5;})
                  .style("fill", "red")

                  node.append("text")
                  .attr("dx", function(d) { return -(d.name.length*3) })
                  .attr("dy", ".65em")
                  .text(function(d) { return d.name });

                  force.on("tick", function() {
                    link.attr("x1", function(d) { return d.source.x; })
                    .attr("y1", function(d) { return d.source.y; })
                    .attr("x2", function(d) { return d.target.x; })
                    .attr("y2", function(d) { return d.target.y; });

                    node.attr("transform", function(d) { return "translate(" + d.x + "," + d.y + ")"; });
                  });
                })();

            }, 500);


        }


        return {
            posts,
            displayPrev,
            displayNext,
            search,
            nextPage,
            prevPage,
            navPage,
            getPost,
            currentState,
            userSearchString,
            searchingString,
            searchHasResults,
            showSearch,
            currentPage,
            totalPages,
            totalPosts,
            checkForFavorite,
            getFavorites,
            createFavorite,
            deleteFavorite,
            words,
            getRelatedWords,
            netWorkString,
            getTermNetwork
        };

    }
});