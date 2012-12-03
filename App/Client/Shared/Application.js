/// <reference path="Event.js" />
/// <reference path="Base.js" />
/// <reference path="http.js" />
/// <reference path="ViewModelStack.js" />
/// <reference path="UrlStack.js" />
/// <reference path="../Vendor/jquery.history.js" />
/// <reference path="../Vendor/jquery.js" />
/// <reference path="../Vendor/knockout.js" />
/// <reference path="ko/requireJsTemplateEngine.js"/>

// An Application object is the root view model of the application.
var Application = Base.inherit({
    init: function () {
        // The view model stack contains a nested master-detail set of view models.
        // For example:
        // [ AppFrame, Vehicles/MasterPage, Vehicles/FillUps ]
        // AppFrame contains Vehicles/MasterPage
        // Vehicles/MasterPage contains Vehicles/FillUps
        this.viewModelStack = ViewModelStack.create(this, http);
        
        // The application's content view model.
        // This will be the bottom of the view model stack (when it's loaded).
        this.content = ko.observable({ templateId: "loading" });

        // Intercept <a> clicks and use the History API to simulate page navigation
        // without ever requesting another HTML page from the server.
        History.init();
        this.onHistoryStateChangeLoadPage();
        this.onAnyClickPushState();

        this.loadInitialPage();

        ko.setTemplateEngine(requireJsTemplateEngine);
        ko.applyBindings(this);
    },
    
    onHistoryStateChangeLoadPage: function() {
        History.Adapter.bind(window, "statechange", function() {
            var state = History.getState();
            this.loadPage(state.url);
        }.bind(this));
    },
    
    onAnyClickPushState: function() {
        document.addEventListener("click", this.handleClick.bind(this), false);
    },

    handleClick: function(event) {
        var clickedLink = event.srcElement || event.target;
        var href = clickedLink.getAttribute("href") || "";
        var rel = clickedLink.getAttribute("rel") || "";
        var isAppLink = href.indexOf("/") === 0;
        var hijaxNotDisabled = !rel.match(/\bnohijax\b/); // Ignore links like <a rel="nohijax">
        if (href && isAppLink && hijaxNotDisabled) {
            event.preventDefault();
            this.navigate(href);
        }
    },

    loadInitialPage: function () {
        this.loadPage(History.getShortUrl(History.getPageUrl()));
    },
    
    loadPage: function (url) {
        // Use the view model stack to "navigate" to the new URL.
        // This will dispose of popped view models and create new
        // view models for the downloaded data, and any parents.
        this.viewModelStack
            .navigate(url)
            .done(function () {
                this.content(this.viewModelStack.rootViewModel());
            }.bind(this));
    },

    navigate: function (url) {
        // Don't actually do any navigation here.
        // The History API's statechange callback defined in onHistoryStateChangeLoadPage
        // will trigger the actual page download.
        History.pushState(null, null, url);
    }
    
});