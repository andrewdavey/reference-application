/// <reference path="../Details/EditVehicleForm.js"/>

var NewVehicleForm = EditVehicleForm.inherit({

    templateId: "Vehicles/New/NewVehicleForm.htm",

    saved: function (data, status, xhr) {
        var vehicleUrl = xhr.getResponseHeader("Location");
        this.app.navigate(vehicleUrl);
    }
    
});