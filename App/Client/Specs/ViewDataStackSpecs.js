/// <reference path="jasmine/jasmine.js" />
/// <reference path="jasmine/specs.js" />
/// <reference path="jasmine/mockHttp.js" />
/// <reference path="~/Client/Shared/ViewDataStack.js"/>

specs.define([], function() {

    describe("ViewDataStack", function () {

        describe("Loading page that has no master page", function() {
            var stack;
            var page = { };

            beforeEach(function() {
                mockHttp.reset();
                mockHttp.get("/page").respondsWith(page);

                stack = ViewDataStack.create(mockHttp);
                stack.navigate("/page");
            });

            it("has page on the stack", function () {
                var array = stack.toArray();
                expect(array).toEqual([page]);
            });

            it("has reference to page via its URL", function() {
                expect(stack.viewDataByUrl["/page"]).toBeTruthy();
            });
        });

        describe("Loading page that has a master page", function() {
            var stack;
            var page = { Master: "/master" };
            var master = {};

            beforeEach(function() {
                mockHttp.reset();
                mockHttp.get("/page").respondsWith(page);
                mockHttp.get("/master").respondsWith(master);
                
                stack = ViewDataStack.create(mockHttp);
                stack.navigate("/page");
            });

            it("page and master on stack", function () {
                var array = stack.toArray();
                expect(array).toEqual([ page, master ]);
            });
        });

        describe("Given page1 -> master loaded, when load page2 which also references 'master'", function () {
            var stack;
            var page1 = { Master: "/master" };
            var page2 = { Master: "/master" };
            var master = {};

            beforeEach(function () {
                mockHttp.reset();
                mockHttp.get("/page1").respondsWith(page1);
                mockHttp.get("/page2").respondsWith(page2);
                mockHttp.get("/master").respondsWith(master);

                stack = ViewDataStack.create(mockHttp);
                stack.navigate("/page1");
                stack.navigate("/page2");
            });

            it("has page2 and master on stack", function () {
                var array = stack.toArray();
                expect(array).toEqual([page2, master]);
            });

            it("no longer has reference to page1", function() {
                expect(stack.viewDataByUrl["/page1"]).toBeFalsy();
            });
        });
        
        describe("Loading page2 that has different master from page1, which was already loaded", function () {
            var stack;
            var page1 = { Master: "/master1" };
            var page2 = { Master: "/master2" };
            var master1 = {};
            var master2 = {};

            beforeEach(function () {
                mockHttp.reset();
                mockHttp.get("/page1").respondsWith(page1);
                mockHttp.get("/page2").respondsWith(page2);
                mockHttp.get("/master1").respondsWith(master1);
                mockHttp.get("/master2").respondsWith(master2);

                stack = ViewDataStack.create(mockHttp);
                stack.navigate("/page1");
                stack.navigate("/page2");
            });

            it("has page2 and master on stack", function () {
                var array = stack.toArray();
                expect(array).toEqual([page2, master2]);
            });
        });
    });
});