/// <reference path="Event.js" />
/// <reference path="Object.js" />
/// <reference path="http.js" />
/// <reference path="ViewModelStack.js" />
/// <reference path="UrlStack.js" />
/// <reference path="../Vendor/jquery.history.js" />
/// <reference path="../Vendor/jquery.js" />
/// <reference path="../Vendor/knockout.js" />

var Application = Object.inherit({
    init: function(document) {
        this.document = document;
        this.viewModelStack = ViewModelStack.create(this, http);
        this.pageLoaded = Event.create();
        this.content = ko.observable({ templateId: "loading" });
    },
    
    start: function() {
        History.init();
        this.onHistoryStateChangeLoadPage();
        this.onAnyClickPushState();
        this.loadPage(History.getPageUrl());
        ko.applyBindings(this);
    },

    onHistoryStateChangeLoadPage: function() {
        History.Adapter.bind(window, "statechange", function() {
            var state = History.getState();
            this.loadPage(state.url);
        }.bind(this));
    },
    
    onAnyClickPushState: function() {
        this.document.addEventListener("click", this.handleClick.bind(this), false);
    },

    handleClick: function(event) {
        var clickedLink = event.srcElement || event.target;
        var href = clickedLink.getAttribute("href");
        if (href && href.indexOf("/") === 0) {
            event.preventDefault();
            History.pushState(null, null, href);
        }
    },

    loadPage: function (url) {
        this.viewModelStack
            .navigate(url)
            .done(function () {
                this.content(this.viewModelStack.rootViewModel());
                this.updateStylesheets();
            }.bind(this));
    },

    navigate: function(url) {
        History.pushState(null, null, url);
    },
    
    updateStylesheets: function () {
        var currentViewModel = this.content();
        var head = document.querySelector("head");
        
        while (currentViewModel) {
            if (currentViewModel.stylesheets) {
                currentViewModel.stylesheets.forEach(function(url) {
                    var existing = document.querySelector("link[href='" + url + "']");
                    if (!existing) {
                        var link = document.createElement("link");
                        link.setAttribute("type", "text/css");
                        link.setAttribute("rel", "stylesheet");
                        link.setAttribute("href", url);
                        head.appendChild(link);
                    }
                });
            }
            
            if (currentViewModel.content) {
                currentViewModel = currentViewModel.content();
            } else {
                currentViewModel = null;
            }
        }
    },
    
    addPageStylesheet: function (stylesheetPath) {
        var stylesheetUrls = styleMap[stylesheetPath];

        var head = document.querySelector("head");
        var links = stylesheetUrls.map(function(url) {
            var link = document.createElement("link");
            link.setAttribute("type", "text/css");
            link.setAttribute("rel", "stylesheet");
            link.setAttribute("href", url);
            head.appendChild(link);
            return link;
        });
        
        this.currentPageStylesheets = links;
    },
    
    removePageStylesheets: function () {
        if (this.currentPageStylesheets) {
            var head = document.querySelector("head");
            this.currentPageStylesheets.forEach(function (linkElement) {
                head.removeChild(linkElement);
            });
            this.currentPageStylesheets = [];
        }
    }
});