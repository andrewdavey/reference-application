/// <reference path="jasmine.js" />
/// <reference path="~/Client/Vendor/knockout.js" />

beforeEach(function () {
    this.addMatchers({
        toBeObservable: function () {
            return ko.isObservable(this.actual);
        }
    });
});
