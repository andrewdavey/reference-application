/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/App/knockout-helpers.js"/>
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
    
    templateId: "Vehicles/Details/Vehicle.htm",
    
    showEditForm: function () {
        var formData = {
            name: this.name(),
            year: this.year(),
            make: this.make(),
            model: this.model(),
            years: this.years,
            save: this.saveCommand
        };
        var form = EditVehicleForm.create(formData, this.app);
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