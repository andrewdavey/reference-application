/// <reference path="~/Infrastructure/Scripts/knockout.js"/>
/// <reference path="~/Modules/Common/httpCommand.js"/>
/// <reference path="~/Modules/Common/Object.js"/>
/// <reference path="VehicleViewModel.js" />

var DashboardViewModel = Object.inherit({

    templateId: "Pages/Dashboard/dashboard.htm",

    init: function (pageData) {
        this.initStatistics(pageData);
        this.initVehicles(pageData);
    },
    
    initStatistics: function(pageData) {
        this.statistics = pageData.statistics;
    },
    
    initVehicles: function(pageData) {
        this.vehicles = ko.observableArray();
        var getVehicles = httpCommand(pageData.vehicles, this);
        getVehicles().then(this.displayVehicles);
    },
    
    displayVehicles: function(response) {
        var vehicles = response.vehicles.map(VehicleViewModel.create);
        this.vehicles(vehicles);
    }
    
});