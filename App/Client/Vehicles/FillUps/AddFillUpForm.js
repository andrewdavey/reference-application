/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Vendor/moment.js"/>
/// <reference path="~/Client/Shared/ko/validation/validation-extender.js" />
/// <reference path="~/Client/Shared/ko/validation/objectWithValidateableProperties.js" />
/// <reference path="~/Client/Shared/Modal.js" />
/// <reference path="~/Client/Shared/Base.js"/>

var AddFillUpForm = Base.inherit({
    
    templateId: "Client/Vehicles/FillUps/AddFillUpForm.htm",
    
    init: function (addCommand, http) {
        this.addCommand = addCommand;
        this.http = http;

        this.initInputs();
        this.initValidation();
        this.initTotalCost();
    },
    
    initInputs: function () {
        var today = moment().sod().toDate();
        this.date = ko.observable(today);
        this.odometer = ko.observable();
        this.pricePerUnit = ko.observable();
        this.totalUnits = ko.observable();
        this.vendor = ko.observable();
        this.transactionFee = ko.observable();
        this.remarks = ko.observable();
    },
    
    initValidation: function () {
        this.date.extend({
            validation: {
                required: "Date is required"
            }
        });
        this.odometer.extend({
            validation: {
                required: "Odometer is required",
                greaterThan: 0
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
            var pricePerUnit = this.pricePerUnit();
            var totalUnits = this.totalUnits();
            var transactionFee = this.transactionFee();
            var total = pricePerUnit * totalUnits + transactionFee;
            return isNaN(total) ? "" : total.toFixed(2);
        }, this);
    },
    
    show: function () {
        this.modal = Modal.create(this);
        return this.modal.showing;
    },
    
    save: function () {
        if (!this.validate()) return;

        var data = this.getSaveData();
        this.http(this.addCommand, data)
            .done(function () {
                var newFillUp = Object.create(data);
                newFillUp.TotalCost = newFillUp.TotalUnits * newFillUp.PricePerUnit + newFillUp.TransactionFee;
                this.close(newFillUp);
            }.bind(this))
            .fail(this.saveFailed.bind(this));
    },
    
    getSaveData: function () {
        return {
            Date: this.date(),
            Odometer: this.odometer(),
            PricePerUnit: this.pricePerUnit(),
            TotalUnits: this.totalUnits(),
            TransactionFee: this.transactionFee(),
            Remarks: this.remarks(),
            Vendor: this.vendor()
        };
    },
    
    cancel: function () {
        this.modal.close();
    },
    
    close: function (fillUpData) {
        this.modal.closeWithResult(fillUpData);
    },

    saveFailed: function (response) {
        if (response.validationErrors) {
            Object.keys(response.validationErrors).forEach(function (key) {
                var property = this[key];
                if (property && property.validation) {
                    property.validation.message(response.validationErrors[key].join(". "));
                }
            }, this);
        } else {
            alert("There was a problem adding the fill-up. Please try again.");
        }
    }
    
}).mixin(objectWithValidateableProperties);