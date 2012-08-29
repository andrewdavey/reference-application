/// <reference path="~/Client/Shared/Base.js"/>
/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="VehicleSummary.js" />

var VehicleSummaryList = Base.inherit({
    
    init: function (vehiclesLink, eventHub) {
        this.http = http;
        this.vehicles = ko.observableArray();
        this.downloadVehicles(vehiclesLink);
        this.subscribeToVehicleUpdatedEvent(eventHub);
    },
    
    downloadVehicles: function (vehiclesLink) {
        this.http(vehiclesLink)
            .then(this.displayVehicles);
    },
    
    displayVehicles: function (response) {
        var vehicles = response.vehicles.map(VehicleSummary.create);
        this.vehicles(vehicles);
    },
    
    subscribeToVehicleUpdatedEvent: function (eventHub) {
        eventHub.subscribe("VehicleUpdated", this.updateVehicleSummary.bind(this));
    },
    
    updateVehicleSummary: function (vehicleData) {
        var isSummaryToUpdate = function(summary) {
            return summary.details === vehicleData.href;
        };
        var summaryToUpdate = this.vehicles().filter(isSummaryToUpdate)[0];
        if (summaryToUpdate) {
            summaryToUpdate.update(vehicleData);
        }
    }
    
});