/// <reference path="Event.js" />
/// <reference path="Object.js" />
/// <reference path="http.js" />
/// <reference path="ViewModelStack.js" />
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
                var viewModels = this.viewModelStack.toArray();
                // e.g. [ FillUps, VehicleMasterPage, App ]
                for (var i = 1; i < viewModels.length; i++) {
                    viewModels[i].content(viewModels[i - 1]);
                }
                this.content(viewModels[viewModels.length - 1]);
            }.bind(this));
    },

    downloadedPageResult: function(pageResult) {
        var app = this;

        this.removePageStylesheets();
        if (pageResult.Stylesheet) app.addPageStylesheet(pageResult.Stylesheet);
        
        require([pageResult.Script], function (page) {
            page.init(pageResult.Data, app);
            document.title = pageResult.Title || "Mileage Stats";
            app.pageLoaded.trigger(pageResult, app);
        });
    },

    downloadFailed: function(xhr) {
        if (xhr.status === 404) {
            this.content({
                templateId: "Errors/NotFound.htm"
            });
        } else {
            this.content({
                templateId: "Errors/ServerError.htm",
                html: xhr.responseText
            });
        }
    },

    navigate: function(url) {
        History.pushState(null, null, url);
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