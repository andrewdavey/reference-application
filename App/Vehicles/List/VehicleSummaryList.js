/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="VehicleSummary.js" />

var VehicleSummaryList = Object.inherit({
    
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