var VehicleViewModel = Object.inherit({
    templateId: "Vehicles/VehiclePage/Vehicle.htm",

    init: function (pageData) {
        this.name = pageData.name;
        this.year = pageData.year;
        this.make = pageData.make;
        this.model = pageData.model;
        this.odometer = pageData.odometer;
        this.photo = pageData.photo;
    }
});
