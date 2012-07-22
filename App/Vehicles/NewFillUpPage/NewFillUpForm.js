﻿/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/moment.js"/>
/// <reference path="~/Infrastructure/Scripts/App/validation/validation-extender.js" />
/// <reference path="~/Infrastructure/Scripts/App/validation/objectWithValidateableProperties.js" />

var NewFillUpForm = Object.inherit({
    
    templateId: "Vehicles/NewFillUpPage/NewFillUpForm.htm",
    
    init: function (data) {
        this.saveLink = data.save;
        this.http = http;

        this.initInputs();
        this.initValidation();
        this.initTotalCost();
    },
    
    initInputs: function () {
        var today = moment().sod().format("LL");
        this.date = ko.observable(today);
        this.odometer = ko.observable();
        this.pricePerUnit = ko.observable();
        this.totalUnits = ko.observable();
        this.vendor = ko.observable();
        this.transactionFee = ko.observable();
        this.remarks = ko.observable();
    },
    
    initValidation: function () {
        this.validate = objectWithValidateableProperties.validate;
        
        this.date.extend({
            validation: {
                required: "Date is required"
            }
        });
        this.odometer.extend({
            validation: {
                required: "Odometer is required",
                pattern: { regex: /^\d+$/, message: "Enter a whole number" }
            }
        });
        this.pricePerUnit.extend({
            validation: {
                required: "Price per unit is required",
                money: true
            }
        });
        this.totalUnits.extend({
            validation: {
                required: "Total units is required"
            }
        });
        this.transactionFee.extend({
            validation: {
                required: "Transaction fee is required",
                money: true
            }
        });
    },
    
    initTotalCost: function () {
        this.totalCost = ko.computed(function () {
            var pricePerUnit = parseFloat(this.pricePerUnit());
            var totalUnits = parseFloat(this.totalUnits());
            var transactionFee = parseFloat(this.transactionFee());
            var total = pricePerUnit * totalUnits + transactionFee;
            return isNaN(total) ? "" : total.toFixed(2);
        }, this);
    },
    
    save: function () {
        if (!this.validate()) return;

        var data = this.getSaveData();
        this.http(this.saveLink, data);
    },
    
    getSaveData: function () {
        return {
            date: moment(this.date()).format("YYYY-MM-DD"),
            odometer: parseInt(this.odometer()),
            pricePerUnit: parseFloat(this.pricePerUnit()),
            totalUnits: parseInt(this.totalUnits(), 10),
            transactionFee: parseFloat(this.transactionFee()),
            remarks: this.remarks(),
            vendor: this.vendor()
        };
    }
    
});