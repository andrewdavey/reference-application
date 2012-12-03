/// <reference path="~/Client/Shared/eventHub.js"/>
/// <reference path="~/Client/Shared/flashMessage.js"/>
/// <reference path="~/Client/Vehicles/Details/Vehicle.js"/>
/// <reference path="~/Client/Vehicles/Details/EditVehicleForm.js"/>
/// <reference path="~/ClientSpecs/Shared/MockHttp.js"/>

describe("Vehicle page", function() {
    var vehicle, mockHttp;
    var pageData = {
        "delete": { "delete": "/vehicles/1" }
    };
    var app = {
        navigate: function() {
        }
    };

    beforeEach(function() {
        mockHttp = MockHttp.create();
        vehicle = Vehicle.create(pageData, app, eventHub, flashMessage, mockHttp.http);
    });

    describe("Using EditVehicleForm to edit vehicle", function () {
        var savedVehicleData;
        beforeEach(function () {
            savedVehicleData = {
                name: "new name",
                year: 2010,
                make: "new make",
                model: "new model"
            };
            // Stub EditVehicleForm to immediately return savedVehicleData.
            EditVehicleForm.create = function() {
                return {
                    show: function() {
                        return $.Deferred().resolve(savedVehicleData);
                    }
                };
            };
            spyOn(eventHub, "publish");
                
            vehicle.showEditForm();
        });

        it("updates `name` from the saved vehicle data", function() {
            expect(vehicle.name()).toBe("new name");
        });
        it("updates `year` from the saved vehicle data", function () {
            expect(vehicle.year()).toBe(2010);
        });
        it("updates `make` from the saved vehicle data", function () {
            expect(vehicle.make()).toBe("new make");
        });
        it("updates `model` from the saved vehicle data", function () {
            expect(vehicle.model()).toBe("new model");
        });
        it("publishes 'VehicleUpdated' event to eventHub", function() {
            expect(eventHub.publish).toHaveBeenCalledWith("VehicleUpdated", savedVehicleData);
        });
    });

    describe("Deleting vehicle", function() {
        beforeEach(function() {
            mockHttp["delete"]("/vehicles/1").respondsWith({ });
            spyOn(app, "navigate");
            spyOn(flashMessage, "show");
            window.confirm = function() { return true; };
            vehicle.confirmDelete();
        });

        it("sends HTTP delete request", function() {
            expect(mockHttp).requested("delete", "/vehicles/1");
        });
        it("navigates application to home page", function() {
            expect(app.navigate).toHaveBeenCalledWith("/");
        });
        it("sets flash message", function() {
            expect(flashMessage.show).toHaveBeenCalledWith("Vehicle deleted");
        });
    });
});