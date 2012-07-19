var VehicleViewModel = Object.inherit({
    templateId: "Vehicles/VehiclePage/Vehicle.htm",

    init: function (pageData) {
        this.name = pageData.Name;
        this.photo = pageData.Photo;
    }
});
