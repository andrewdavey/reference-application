/// <reference path="jasmine/jasmine.js" />
/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/mockHttp.js"/>

specs.define(["Vehicles/List"], function(module) {
    var VehicleSummaryList = module.VehicleSummaryList;

    describe("VehicleSummaryList", function () {
        var vehicles = [
            { year: 2012, make: "Test1", model: "Car1", details: { }, fillUps: { }, reminders: { } },
            { year: 2011, make: "Test2", model: "Car2", details: { }, fillUps: { }, reminders: { } }
        ];
        mockHttp.get("/vehicles").respondsWith({ vehicles: vehicles });
        
        var vehiclesLink = { method: "get", url: "/vehicles" };
        var summaryList = VehicleSummaryList.create(vehiclesLink);

        it("downloads vehicles", function () {
            var vehicles = summaryList.vehicles();
            expect(vehicles[0].year).toEqual(2012);
            expect(vehicles[0].make).toEqual("Test1");
            expect(vehicles[0].model).toEqual("Car1");
            expect(vehicles[1].year).toEqual(2011);
            expect(vehicles[1].make).toEqual("Test2");
            expect(vehicles[1].model).toEqual("Car2");
        });
    });
    
});