/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>

var FillUp = Object.inherit({
    init: function (data) {
        var units = ["gallon", "litre"];
        var currency = "$";
        
        this.date = new Date(Date.parse(data.Date));
        this.totalCost = currency + data.TotalCost.toFixed(2);
        this.totalUnits = data.TotalUnits;
        this.unitOfMeasure = units[data.UnitOfMeasure];
        this.pricePerUnit = currency + data.PricePerUnit.toFixed(2);
        this.vendor = data.Vendor;
        this.odometer = data.Odometer;
        this.remarks = data.Remarks;
    }
});