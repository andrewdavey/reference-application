/// <reference path="Event.js" />
/// <reference path="Object.js" />
/// <reference path="../Vendor/jquery.history.js" />
/// <reference path="../Vendor/jquery.js" />
/// <reference path="../Vendor/knockout.js" />

var Application = Object.inherit({
    init: function(document) {
        this.document = document;
        this.viewModel = ko.observable({ templateId: "loading" });
        this.pageLoaded = Event.create();
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

    loadPage: function(url) {
        var request = $.ajax({
            type: "GET",
            url: url,
            dataType: "json",
            context: this
        });
        request.done(this.downloadedPageResult)
            .fail(this.downloadFailed);
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
            this.setViewModel({
                templateId: "Errors/NotFound.htm"
            });
        } else {
            this.setViewModel({
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
    },
    
    setViewModel: function(viewModel) {
        this.viewModel(viewModel);
    }
});