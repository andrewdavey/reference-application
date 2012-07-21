/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>

var VehicleSummary = Object.inherit({
    init: function (data) {
        this.name = data.name;
        this.photo = data.photo ? data.photo.url : "";
        this.year = data.year ? data.year : "";
        this.make = data.make ? data.make : "";
        this.model = data.model ? data.model : "";
        this.averageFuelEfficiency = data.averageFuelEfficiency;
        this.averageCostToDrive = data.averageCostToDrive;
        this.averageCostPerMonth = data.averageCostPerMonth;
        this.details = data.details.url;
        this.fillUps = data.fillUps.url;
        this.reminders = data.reminders.url;
    }
})