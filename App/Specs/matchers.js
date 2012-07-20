/// <reference path="jasmine.js" />
/// <reference path="../Infrastructure/Scripts/Vendor/knockout.js" />

beforeEach(function () {
    this.addMatchers({
        toBeObservable: function () {
            return ko.isObservable(this.actual);
        }
    });
});
