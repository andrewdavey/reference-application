/// <reference path="UrlStack.js" />
/// <reference path="http.js"/>

var ViewModelStack = UrlStack.inherit({
    
    init: function (app, http) {
        this.app = app;
        this.http = http;
        this.viewModels = {}; // { "/page/url": view-model, ... }
        this.stylesheets = {}; // { "/page/url": [ array-of-CSS-URLS, ... ], ... }
        
        UrlStack.init.call(this, this.downloadUrl, this.onPop);
    },
    
    downloadUrl: function (url) {
        return this.http({ method: "GET", url: url, accept: "application/x-page+json" });
    },

    navigate: function (newUrl) {
        var navigation = UrlStack.navigate.call(this, newUrl);
        this.hasSetPageTitle = false;
        navigation.done(function (commonParentUrl) {
            if (commonParentUrl) {
                // The parent view model has not changed, but we've loaded a new child.
                // So we need to set the parent's content property.
                this.updateParentContent(commonParentUrl);
            }
        });
        return navigation;
    },
    
    updateParentContent: function (parentUrl) {
        // Child URL will be after the parent URL in the this.urls array.
        var childUrl = this.urls[1 + this.urls.indexOf(parentUrl)];
        var childViewModel = this.viewModels[childUrl];
        if (childViewModel) {
            var parentViewModel = this.viewModels[parentUrl];
            parentViewModel.content(childViewModel);
        }
    },
    
    processDownloadResponse: function (response) {
        // Call prototype
        var process = UrlStack.processDownloadResponse.apply(this, arguments);

        var useStylesheets = function() {
            this.stylesheets[response.url] = response.body.stylesheets.map(function (url) {
                return this.addStylesheet(url);
            }, this);
        };
        
        var downloadModule = function () {
            // Use require.js to download the view model module.
            var moduleResult = $.Deferred();
            var modulePath = response.body.Script;
            require([modulePath], function (module) {
                moduleResult.resolveWith(this, [module]);
            }.bind(this));
            return moduleResult;
        }.bind(this);
        
        var createViewModel = function (module) {
            var viewData = response.body.Data;
            var viewModel = module.init(viewData, this.app);
            this.viewModels[response.url] = viewModel;
            return viewModel;
        }.bind(this);

        var setViewModelContent = function(viewModel) {
            if (response.childUrl) {
                var childViewModel = this.viewModels[response.childUrl];
                viewModel.content(childViewModel);
            }
        }.bind(this);

        var setPageTitle = function () {
            if (!this.hasSetPageTitle) {
                document.title = response.body.title;
                this.hasSetPageTitle = true;
            }
        }.bind(this);
        
        return process
            .pipe(setPageTitle)
            .pipe(useStylesheets)
            .pipe(downloadModule)
            .pipe(createViewModel)
            .pipe(setViewModelContent)
            .pipe(function () {
                return response;
            });
    },
    
    onUrlPopped: function (url) {
        this.disposeViewModelForUrl(url);
        this.removeStylesheetForUrl(url);
    },
    
    disposeViewModelForUrl: function (url) {
        var viewModel = this.viewModels[url];
        if (typeof viewModel.dispose === "function") {
            viewModel.dispose();
        }
        delete this.viewModels[url];
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
            delete link;
        };
        return { remove: remove };
    },
    
    removeStylesheetForUrl: function (url) {
        var stylesheetsForUrl = this.stylesheets[url];
        stylesheetsForUrl.forEach(function (stylesheet) {
            stylesheet.remove();
        });
    },
    
    rootViewModel: function () {
        return this.viewModels[this.urls[0]];
    }
});