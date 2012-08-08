/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="../List/VehicleSummaryList.js" />

var MasterPage = Object.inherit({
    
    init: function (viewData, eventHub) {
        this.vehicles = VehicleSummaryList.create(viewData.vehicles, eventHub);
        this.content = ko.observable();
    },
    
    templateId: "Vehicles/MasterPage/MasterPage.htm"
    
});