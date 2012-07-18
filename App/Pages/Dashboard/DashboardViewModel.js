/// <reference path="~/Modules/Common/httpCommand.js"/>
/// <reference path="~/Modules/Common/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/knockout.js"/>
/// <reference path="Vehicle.js" />

var DashboardViewModel = Object.inherit({

    templateId: "Pages/Dashboard/dashboard.htm",

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