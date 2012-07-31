/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/knockout-helpers.js"/>

var VehicleSummary = Object.inherit({
    init: function(data) {
        this.name = ko.observable(data.name);
        this.photo = data.photo ? data.photo.url : "";
        this.year = ko.observable(data.year ? data.year : "");
        this.make = ko.observable(data.make ? data.make : "");
        this.model = ko.observable(data.model ? data.model : "");
        this.averageFuelEfficiency = data.averageFuelEfficiency;
        this.averageCostToDrive = data.averageCostToDrive;
        this.averageCostPerMonth = data.averageCostPerMonth;
        this.details = data.details.url;
        this.fillUps = data.fillUps.url;
        this.reminders = data.reminders.url;
    },

    update: function (data) {
        updateObservableProperties(data, this);
    }
});