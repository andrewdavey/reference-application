/// <reference path="jasmine.js" />
/// <reference path="matchers.js" />
/// <reference path="../Profile/ProfileForm.js" />
/// <reference path="../Infrastructure/Scripts/Vendor/jquery.js" />

function deferredFactory(value) {
    return function(context) {
        return $.Deferred().resolveWith(context, [value]);
    };
}

describe("Profile form", function () {

    describe("Properties", function() {
        var pageData = {
            countries: deferredFactory({ countries: ["US", "UK"] }),
            save: deferredFactory()
        };
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
        var pageData = {
            countries: deferredFactory({ countries: ["US", "UK"] }),
            save: function () {
                saveCommandCalled = true;
                return $.Deferred().resolve();
            }
        };
        var form = ProfileForm.create(pageData);
        
        form.name("");
        form.save();

        it("has name validation error", function () {
            expect(form.name.validation.message()).toBe("Name is required");
        });
        
        it("doesn't call the save command", function() {
            expect(saveCommandCalled).toBeFalsy();
        });
    });

    describe("Save with valid inputs", function () {
        var savedData;
        var savedCalled;
        var pageData = {
            countries: deferredFactory({ countries: ["US", "UK"] }),
            save: function (data) {
                savedData = data;
                return $.Deferred().resolve();
            } 
        };
        
        var form = ProfileForm.create(pageData);
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