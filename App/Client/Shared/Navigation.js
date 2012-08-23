/// <reference path="../Vendor/knockout.js" />
/// <reference path="Object.js" />

var Navigation = Object.inherit({

    init: function (app) {
        this.app = app;
        this.links = ko.observableArray();
        app.pageLoaded.subscribe(this.resetLinks.bind(this));
    },

    resetLinks: function (pageData) {
        this.links(pageData.navigation);
    }
    
});