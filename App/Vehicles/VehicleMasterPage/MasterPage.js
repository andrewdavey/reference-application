/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="../List/VehicleSummaryList.js" />

var MasterPage = Object.inherit({
    
    init: function (viewData) {
        this.vehicles = VehicleSummaryList.create(viewData.vehicles).vehicles;
        this.content = ko.observable();
    },
    
    templateId: "Vehicles/VehicleMasterPage/MasterPage.htm"
    
});