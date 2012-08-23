/// <reference path="~/Client/Vendor/moment.js"/>
/// <reference path="~/Client/Shared/Object.js"/>

var FillUp = Object.inherit({
    init: function (data) {
        var units = ["gallon", "litre"];
        var currency = "$";
        
        this.date = moment(data.Date).format("L");
        this.totalCost = currency + data.TotalCost.toFixed(2);
        this.totalUnits = data.TotalUnits;
        this.unitOfMeasure = units[data.UnitOfMeasure];
        this.pricePerUnit = currency + data.PricePerUnit.toFixed(2);
        this.vendor = data.Vendor;
        this.odometer = data.Odometer;
        this.remarks = data.Remarks;
    }
});