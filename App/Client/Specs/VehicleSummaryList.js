/// <reference path="jasmine/jasmine.js" />
/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/mockHttp.js"/>
/// <reference path="~/Client/Shared/EventHub.js" />

specs.define(["Client/Vehicles/List"], function (module) {
    var VehicleSummaryList = module.VehicleSummaryList;
    var eventHub = EventHub.create();

    describe("VehicleSummaryList", function () {
        var vehicles = [
            { name: "Vehicle1", year: 2012, make: "Test1", model: "Model1", details: { url: "/vehicles/1" }, fillUps: { }, reminders: { } },
            { name: "Vehicle2", year: 2011, make: "Test2", model: "Model2", details: { url: "/vehicles/2" }, fillUps: { }, reminders: { } }
        ];
        mockHttp.get("/vehicles").respondsWith({ vehicles: vehicles });
        
        var vehiclesLink = { method: "get", url: "/vehicles" };
        var summaryList = VehicleSummaryList.create(vehiclesLink, eventHub);

        it("downloads vehicles", function () {
            var vehicles = summaryList.vehicles();
            expect(vehicles[0].year()).toEqual(2012);
            expect(vehicles[0].make()).toEqual("Test1");
            expect(vehicles[0].model()).toEqual("Model1");
            expect(vehicles[1].year()).toEqual(2011);
            expect(vehicles[1].make()).toEqual("Test2");
            expect(vehicles[1].model()).toEqual("Model2");
        });

        describe("when VehicleUpdated event published", function () {
            var eventData = {
                href: "/vehicles/1",
                name: "New Name",
                year: 2000,
                make: "New Make",
                model: "New Model"
            };
            beforeEach(function() {
                eventHub.publish("VehicleUpdated", eventData);
            });
            it("updates the vehicle summary", function() {
                var vehicles = summaryList.vehicles();
                expect(vehicles[0].name()).toEqual("New Name");
                expect(vehicles[0].year()).toEqual(2000);
                expect(vehicles[0].make()).toEqual("New Make");
                expect(vehicles[0].model()).toEqual("New Model");
            });
        });
    });
    
});