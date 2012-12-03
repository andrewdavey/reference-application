/// <reference path="jasmine.js" />
/// <reference path="~/Client/Vendor/knockout.js" />

beforeEach(function () {

    this.addMatchers({
        toBeObservable: function () {
            var ko = require("Client/Vendor/knockout");
            return ko.isObservable(this.actual);
        }
    });
    
    this.addMatchers({
        requested: function (method, url) {
            var http = this.actual;
            return http.wasRequestMade(method, url );
        }
    });
    
});