/// <reference path="~/Infrastructure/Scripts/App/httpCommand.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="Vehicle.js" />

var DashboardViewModel = Object.inherit({

    templateId: "Dashboard/dashboard.htm",

    init: function (pageData) {
        this.initStatistics(pageData);
        this.initVehicles(pageData);
        this.initReminders(pageData);
    },
    
    initStatistics: function(pageData) {
        this.statistics = pageData.statistics;
    },
    
    initVehicles: function(pageData) {
        this.vehicles = ko.observableArray();
        this.addVehicleUrl = pageData.addVehicle;
        var getVehicles = httpCommand(pageData.vehicles, this);
        getVehicles().then(this.displayVehicles);
    },
    
    initReminders: function(pageData) {
        this.reminders = pageData.reminders;
    },
    
    displayVehicles: function(response) {
        var vehicles = response.vehicles.map(Vehicle.create);
        this.vehicles(vehicles);
    }
    
});