/// <reference path="~/Client/Shared/linkUtils.js" />

describe("linkUtils", function () {

    describe("getMethod", function () {
        it("returns name of first property in object", function () {
            expect(linkUtils.getMethod({ get: "/URL" })).toEqual("get");
        });
    });

    describe("getUrl", function () {
        it("returns first property value in object", function () {
            expect(linkUtils.getUrl({ get: "/URL" })).toEqual("/URL");
        });
    });

});