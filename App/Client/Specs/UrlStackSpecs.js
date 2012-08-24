/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/matchers.js"/>
/// <reference path="jasmine/jasmine.js"/>
/// <reference path="jasmine/mockHttp.js"/>
/// <reference path="../Shared/UrlStack.js"/>

specs.define([], function() {

    describe("UrlStack", function() {
        var stack;

        var stubUrlData = {
            "/page": {},
            "/child": { parent: "/parent" },
            "/child2": { parent: "/parent" },
            "/parent": {}
        };
        var download = function(url) {
            return $.Deferred().resolveWith(this, [stubUrlData[url]]);
        };
        var popped;
        var onUrlPopped = function(url) {
            popped.push(url);
        };
        
        beforeEach(function() {
            popped = [];
            stack = UrlStack.create(download, onUrlPopped);
        });

        describe("empty UrlStack", function() {
            it("is empty", function () {
                expect(stack.urls).toEqual([]);
            });

            describe("navigate to /page", function () {
                var navigation;
                beforeEach(function() {
                    navigation = stack.navigate("/page");
                });

                it("downloads /page data", function () {
                    navigation.then(function() {
                        expect(stack.downloaded["/page"]).toBeTruthy();
                    });
                });

                it("contains /page", function() {
                    navigation.then(function() {
                        expect(stack.urls).toEqual(["/page"]);
                    });
                });
            });

            describe("navigate to /child, which has parent /parent", function() {
                var navigation;
                beforeEach(function () {
                    navigation = stack.navigate("/child");
                });

                it("contains /parent and /child", function() {
                    navigation.then(function() {
                        expect(stack.urls).toEqual(["/parent", "/child"]);
                    });
                });

                it("downloads /child data", function () {
                    navigation.then(function () {
                        expect(stack.downloaded["/child"]).toBeTruthy();
                    });
                });

                it("downloads /parent data", function () {
                    navigation.then(function () {
                        expect(stack.downloaded["/parent"]).toBeTruthy();
                    });
                });
            });

            describe("navigate from /page to /child", function() {
                beforeEach(function() {
                    stack.navigate("/page");
                    stack.navigate("/child");
                });

                it("pops /page", function () {
                    expect(popped).toEqual(["/page"]);
                });

                it("contains /parent and /child", function() {
                    expect(stack.urls).toEqual(["/parent", "/child"]);
                });
            });

            describe("navigate from /child to /child2", function() {
                beforeEach(function () {
                    stack.navigate("/child");
                    stack.navigate("/child2");
                });

                it("only pops /child, not /parent", function() {
                    expect(popped).toEqual(["/child"]);
                });
            });
        });
    });
    
});