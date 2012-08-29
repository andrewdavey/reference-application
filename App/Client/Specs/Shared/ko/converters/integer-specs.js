/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="../../../jasmine/specs.js"/>
/// <reference path="../../../jasmine/jasmine.js"/>

specs.define(["Client/Shared"], function() {

    describe("integer converter", function () {
        var underlying, converted;

        beforeEach(function () {
            underlying = ko.observable();
            converted = underlying.convert("integer");
        });
        
        it("converts string into integer", function() {
            converted("2");
            expect(underlying()).toBe(2);
        });

        describe("writing invalid integer", function() {
            beforeEach(function() {
                converted("fail");
            });
            it("sets underlying value to NaN", function() {
                expect(isNaN(underlying())).toBe(true);
            });
            it("returns the invalid value", function() {
                expect(converted()).toBe("fail");
            });
        });

        describe("writing 'fail' and then writing '3'", function () {
            beforeEach(function() {
                converted("fail");
                converted("3");
            });
            it("sets the underlying observable to the valid value", function () {
                expect(underlying()).toBe(3);
            });
            it("returns the valid string", function() {
                expect(converted()).toBe("3");
            });
        });

        describe("writing 'fail' to converted, and then writing 4 to the underlying", function() {
            beforeEach(function() {
                converted("fail");
                underlying(4);
            });
            it("updates converted to be '4'", function() {
                expect(converted()).toBe("4");
            });
        });
    });

});