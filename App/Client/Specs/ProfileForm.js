/// <reference path="jasmine/jasmine.js" />
/// <reference path="jasmine/matchers.js" />
/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/mockHttp.js"/>

specs.define(["Client/Profile"], function (profile) {
    var ProfileForm = profile.ProfileForm;


    describe("Profile form", function() {

        var pageData = {
            countries: { method: "get", url: "/countries" },
            save: { method: "post", url: "/save" }
        };


        describe("Properties", function() {
            mockHttp.reset();
            mockHttp.get("/countries").respondsWith({ countries: ["US", "UK"] });

            var form = ProfileForm.create(pageData);

            it("has name input", function() {
                expect(form.name).toBeObservable();
            });

            it("has country input", function() {
                expect(form.country).toBeObservable();
            });

            it("has countries array", function() {
                expect(form.countries()).toEqual(["US", "UK"]);
            });
        });


        describe("Save profile with empty name", function () {
            var saveCommandCalled = false;
            mockHttp.reset();
            mockHttp.get("/countries").respondsWith({ countries: ["US", "UK"] });
            mockHttp.post("/save").calls(function () {
                saveCommandCalled = true;
            });

            var form = ProfileForm.create(pageData);

            form.name("");
            form.save();

            it("has name validation error", function() {
                expect(form.name.validation.message()).toBe("Name is required");
            });

            it("doesn't call the save command", function() {
                expect(saveCommandCalled).toBeFalsy();
            });
        });


        describe("Save with valid inputs", function () {
            var savedData;
            mockHttp.reset();
            mockHttp.get("/countries").respondsWith({ countries: ["US", "UK"] });
            mockHttp.post("/save").calls(function (_, data) {
                savedData = data;
            });

            var form = ProfileForm.create(pageData);
            var savedCalled;
            form.saved = function () { savedCalled = true; };
            form.name("John");
            form.country("US");
            form.save();

            it("saves name", function() {
                expect(savedData.name).toEqual("John");
            });

            it("saves country", function() {
                expect(savedData.country).toEqual("US");
            });

            it("calls `saved` method after save", function() {
                expect(savedCalled).toBeTruthy();
            });
        });

    });

});