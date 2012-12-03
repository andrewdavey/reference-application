/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/ko/convert.js"/>
/// <reference path="~/Client/Shared/ko/converters/money.js"/>

describe("money converter", function () {
    var underlying, converted;

    beforeEach(function () {
        underlying = ko.observable();
        converted = underlying.convert("money");
    });
        
    it("converts string into number", function() {
        converted("2.50");
        expect(underlying()).toBe(2.5);
    });

    it("displays formatted amount after update", function() {
        converted("1");
        expect(converted()).toBe("1.00");
    });

    describe("writing invalid money string", function() {
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
        it("updates converted to be '3.00'", function() {
            expect(converted()).toBe("3.00");
        });
    });

    describe("writing 'fail' to converted, and then writing 4 to the underlying", function() {
        beforeEach(function() {
            converted("fail");
            underlying(4);
        });
        it("updates converted to be '4.00'", function() {
            expect(converted()).toBe("4.00");
        });
    });
});