/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/httpCommand.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="VehicleSummary.js" />

var VehicleSummaryList = Object.inherit({
    init: function (vehiclesHref) {
        this.vehicles = ko.observableArray();
        var getVehicles = httpCommand(vehiclesHref, this);
        getVehicles().then(this.displayVehicles);
    },
    
    displayVehicles: function (response) {
        var vehicles = response.vehicles.map(VehicleSummary.create);
        this.vehicles(vehicles);
    }
});