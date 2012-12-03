/// <reference path="~/Client/Profile/ProfileForm.js"/>
/// <reference path="~/ClientSpecs/Shared/MockHttp.js"/>

describe("Profile form", function() {

    var pageData = {
        countries: { method: "get", url: "/countries" },
        save: { method: "post", url: "/save" }
    };
    var mockHttp, form;

    beforeEach(function() {
        mockHttp = MockHttp.create();
        mockHttp.get("/countries").respondsWith({ countries: ["US", "UK"] });
        form = ProfileForm.create(pageData, mockHttp.http);
    });
    
    describe("Properties", function () {
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
        var saveCommandCalled;
        
        beforeEach(function () {
            saveCommandCalled = false;
            mockHttp.post("/save").calls(function() {
                saveCommandCalled = true;
            });
            form.name("");
            form.save();
        });
        
        it("has name validation error", function() {
            expect(form.name.validation.message()).toBe("Name is required");
        });

        it("doesn't call the save command", function() {
            expect(saveCommandCalled).toBeFalsy();
        });
    });


    describe("Save with valid inputs", function () {
        var savedData;
        var savedCalled;
        beforeEach(function () {
            mockHttp.post("/save").calls(function(_, data) {
                savedData = data;
            });

            form.saved = function() { savedCalled = true; };
            form.name("John");
            form.country("US");
            form.save();
        });
        
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