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
            }.bind(this));
    },

    navigate: function(url) {
        History.pushState(null, null, url);
    },
    
    addStylesheet: function (stylesheetUrl) {
        var head = document.querySelector("head");
        var link = document.createElement("link");
        link.setAttribute("type", "text/css");
        link.setAttribute("rel", "stylesheet");
        link.setAttribute("href", stylesheetUrl);
        head.appendChild(link);
        
        // Return an object we can use later to remove the stylesheet.
        var remove = function () {
            head.removeChild(link);
        };
        return { remove: remove };
    }
});