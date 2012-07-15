/// <reference path="~/Modules/Common/Object.js"/>

var VehicleViewModel = Object.inherit({
    templateId: "Pages/Vehicle/Vehicle.htm",
    
    init: function (pageData) {
        this.name = pageData.vehicle.Name;
    }
});

var init = function (pageData, app) {
    app.setViewModel(VehicleViewModel.create(pageData));
};