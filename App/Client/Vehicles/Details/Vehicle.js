/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/Object.js"/>
/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Shared/knockout-helpers.js"/>
/// <reference path="EditVehicleForm.js" />

var Vehicle = Object.inherit({

    init: function (viewData, app, eventHub, flashMessage) {
        this.app = app;
        this.eventHub = eventHub;
        this.flashMessage = flashMessage;
        this.http = http;
        this.years = viewData.years;
        this.initHttpCommands(viewData);
        this.initDisplayProperties(viewData);
    },
    
    initHttpCommands: function (viewData) {
        this.saveCommand = viewData.save;
        this.deleteCommand = viewData["delete"];
    },
    
    initDisplayProperties: function (viewData) {
        this.name = ko.observable(viewData.name);
        this.year = ko.observable(viewData.year);
        this.make = ko.observable(viewData.make);
        this.model = ko.observable(viewData.model);
        this.odometer = viewData.odometer;
        this.photo = viewData.photo;
    },
    
    templateId: "Client/Vehicles/Details/Vehicle.htm",
    
    showEditForm: function () {
        var formData = {
            name: this.name(),
            year: this.year(),
            make: this.make(),
            model: this.model(),
            years: this.years,
            save: this.saveCommand
        };
        var form = EditVehicleForm.create(formData, this.flashMessage);
        form.show()
            .done(this.updateProperties.bind(this));
    },
    
    updateProperties: function (updatedVehicleData) {
        if (!updatedVehicleData) return;
        
        updateObservableProperties(updatedVehicleData, this);
        this.eventHub.publish("VehicleUpdated", updatedVehicleData);
    },
    
    confirmDelete: function () {
        if (!confirm("Delete " + this.name() + "?")) return;
        this.http(this.deleteCommand)
            .done(this.afterDelete);
    },
    
    afterDelete: function () {
        this.flashMessage.show("Vehicle deleted");
        this.app.navigate("/");
    }
});