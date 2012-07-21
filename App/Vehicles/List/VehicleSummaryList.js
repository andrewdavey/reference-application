/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="VehicleSummary.js" />

var VehicleSummaryList = Object.inherit({
    
    init: function (vehiclesLink) {
        this.http = http;
        this.vehicles = ko.observableArray();
        this.downloadVehicles(vehiclesLink);
    },
    
    downloadVehicles: function (vehiclesLink) {
        this.http(vehiclesLink)
            .then(this.displayVehicles);
    },
    
    displayVehicles: function (response) {
        var vehicles = response.vehicles.map(VehicleSummary.create);
        this.vehicles(vehicles);
    }
    
});