/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/jasmine.js"/>

specs.define(["Infrastructure/Scripts/App"], function() {

    describe("observable as integer", function() {
        var observable;

        beforeEach(function() {
            observable = ko.observable().extend({ as: "integer" });
        });
        
        it("has `asString` observable", function () {
            observable(1);
            expect(observable.asString()).toBe("1");
        });

        it("converts string written to `asString` into integer", function() {
            observable.asString("2");
            expect(observable()).toBe(2);
        });

        describe("writing invalid integer to `asString`", function() {
            beforeEach(function() {
                observable.asString("fail");
            });
            it("sets main observable value to undefined", function() {
                expect(observable()).toBeNull();
            });
            it("saves the invalid value in `asString`", function() {
                expect(observable.asString()).toBe("fail");
            });
        });

        describe("writing 'fail' to `asString` and then writing '3' to `asString`", function () {
            beforeEach(function() {
                observable.asString("fail");
                observable.asString("3");
            });
            it("sets the main observable to the valid value", function () {
                expect(observable()).toBe(3);
            });
            it("sets `asString` to be the valid string", function() {
                expect(observable.asString()).toBe("3");
            });
        });

        describe("writing 'fail' to `asString` and then writing 4 to the main observable", function() {
            beforeEach(function() {
                observable.asString("fail");
                observable(4);
            });
            it("updates `asString` to be '4'", function() {
                expect(observable.asString()).toBe("4");
            });
        });
    });

});