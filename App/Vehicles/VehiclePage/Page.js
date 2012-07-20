/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="Vehicle.js"/>
/// <reference path="../List/VehicleSummaryList.js"/>

var Page = Object.inherit({
    
    templateId: "Vehicles/VehiclePage/Page.htm",

    init: function (pageData) {
        this.vehicle = Vehicle.create(pageData);
        this.vehicles = VehicleSummaryList.create(pageData.vehicles);
    }
    
});