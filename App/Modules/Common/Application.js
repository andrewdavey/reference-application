/// <reference path="Event.js" />
/// <reference path="Object.js" />
/// <reference path="~/Infrastructure/Scripts/jquery.history.js" />
/// <reference path="~/Infrastructure/Scripts/jquery.js" />
/// <reference path="~/Infrastructure/Scripts/knockout.js" />

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
        if (href) {
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
        require([pageResult.script], function(page) {
            page.init(pageResult.data, app);
            document.title = pageResult.title;
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

    setViewModel: function(viewModel) {
        var app = this;
        require(["template!" + viewModel.templateId], function() {
            app.viewModel(viewModel);
        });
    }
});