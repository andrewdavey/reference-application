/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="EditVehicleForm.js" />

var Vehicle = Object.inherit({

    init: function (viewData, app) {
        this.viewData = viewData;
        this.app = app;
        this.http = http;
        this.name = ko.observable(viewData.name);
        this.year = ko.observable(viewData.year);
        this.make = ko.observable(viewData.make);
        this.model = ko.observable(viewData.model);
        this.odometer = viewData.odometer;
        this.photo = viewData.photo;
        this.deleteCommand = viewData["delete"];
    },
    
    templateId: "Vehicles/VehiclePage/Vehicle.htm",
    
    showEditForm: function () {
        var formData = {
            name: this.name(),
            year: this.year(),
            make: this.make(),
            model: this.model(),
            years: this.viewData.years,
            save: this.viewData.save
        };
        var form = EditVehicleForm.create(formData, this.app);
        form.show()
            .done(this.updateProperties.bind(this));
    },
    
    updateProperties: function(updatedVehicleData) {
        this.name(updatedVehicleData.Name);
        this.year(updatedVehicleData.Year);
        this.make(updatedVehicleData.MakeName);
        this.model(updatedVehicleData.ModelName);
    },
    
    confirmDelete: function () {
        if (!confirm("Delete " + this.name() + "?")) return;
        this.http(this.deleteCommand)
            .done(this.afterDelete);
    },
    
    afterDelete: function () {
        this.app.flashMessage.show("Vehicle deleted");
        this.app.navigate("/");
    }
});