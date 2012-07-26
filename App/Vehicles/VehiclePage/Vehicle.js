/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>

var Vehicle = Object.inherit({

    init: function (pageData, app) {
        this.app = app;
        this.http = http;
        this.name = pageData.name;
        this.year = pageData.year;
        this.make = pageData.make;
        this.model = pageData.model;
        this.odometer = pageData.odometer;
        this.photo = pageData.photo;
        this.deleteCommand = pageData["delete"];
    },
    
    templateId: "Vehicles/VehiclePage/Vehicle.htm",
    
    showEditForm: function () {
        
    },
    
    confirmDelete: function () {
        if (!confirm("Delete " + this.name + "?")) return;
        this.http(this.deleteCommand)
            .done(this.afterDelete);
    },
    
    afterDelete: function () {
        this.app.flashMessage.show("Vehicle deleted");
        this.app.navigate("/");
    }
});