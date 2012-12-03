/// <reference path="~/Client/Shared/flashMessage.js"/>
/// <reference path="~/Client/Shared/eventHub.js"/>
/// <reference path="~/Client/Dashboard/Dashboard.js"/>
/// <reference path="~/ClientSpecs/Shared/MockHttp.js"/>

describe("Dashboard", function () {
    var pageData = {
        profile: { method: "get", url: "/profile" },
        vehicles: { method: "get", url: "/vehicles" },
        addVehicle: { method: "get", url: "/vehicles/add" },
        statistics: {}
    };
    var dashboard, mockHttp;
    
    beforeEach(function () {
        mockHttp = MockHttp.create();
        mockHttp.get("/vehicles").respondsWith({ vehicles: [] });
        mockHttp.get("/countries").respondsWith({ countries: [] });
    });

    describe("given profile is not complete", function() {
        beforeEach(function () {
            var incompleteProfile = {
                name: "",
                country: "",
                hasRegistered: false,
                countries: { method: "get", url: "/countries" }
            };
            mockHttp.get("/profile").respondsWith(incompleteProfile);
            dashboard = Dashboard.create(pageData, flashMessage, eventHub, mockHttp.http);
        });

        it("has a profile view model", function() {
            expect(dashboard.profile()).toBeTruthy();
        });

        describe("profile form saved", function() {
            beforeEach(function() {
                dashboard.profile().onSaved.trigger();
            });

            it("removes the profile view model", function() {
                expect(dashboard.profile()).toBeFalsy();
            });

            it("shows flash message", function() {
                expect(dashboard.flashMessage.message()).toBe("Profile saved");
            });
        });
    });

    describe("given profile is complete", function () {
        var completeProfile = {
            hasRegistered: true,
            countries: { method: "get", url: "/countries" }
        };

        beforeEach(function() {
            mockHttp.get("/profile").respondsWith(completeProfile);
            dashboard = Dashboard.create(pageData, flashMessage, eventHub, mockHttp.http);
        });

        it("doesn't have a profile view model", function() {
            expect(dashboard.profile()).toBeFalsy();
        });
    });
});