/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/moment.js"/>

var validators = {
    required: function(value, message) {
        return value ? true : message;
    },
    pattern: function (value, options) {
        return options.regex.test(value) ? true : options.message;
    },
    money: function (value) {
        return validators.pattern(
            value,
            {
                regex: /^(\d*\.\d\d|\d+)$/,
                message: "Please enter an amount of money"
            }
        );
    }
};

ko.extenders.validation = function (target, options) {
    target.validation = {
        message: ko.observable(),
        isInvalid: ko.observable(false),
        validate: function () {
            var isValid = true;
            var value = target();
            for (var propertyName in options) {
                if (!options.hasOwnProperty(propertyName)) continue;
                
                var result = validators[propertyName](value, options[propertyName]);
                if (result === true) {
                    target.validation.message("");
                } else {
                    target.validation.message(result);
                    isValid = false;
                    break;
                }
            }
            target.validation.isInvalid(!isValid);
            return isValid;
        }
    };
    
    target.subscribe(function() {
        target.validation.validate();
    });
    
    return target;
};


ko.bindingHandlers.validation = {
    init: function (element) {
        var controlsDiv = element.querySelector(".controls");
        var messageSpan = document.createElement("span");
        messageSpan.setAttribute("class", "help-inline validation-error-message");
        controlsDiv.appendChild(messageSpan);
    },
    update: function (element, valueAccessor) {
        var validation = valueAccessor().validation;
        
        ko.bindingHandlers.css.update(element, function () {
            return { error: validation.isInvalid };
        });

        var span = element.querySelector(".validation-error-message");
        ko.bindingHandlers.text.update(span, validation.message);
    }
};

var NewFillUpForm = Object.inherit({
    
    templateId: "Vehicles/NewFillUpPage/NewFillUpForm.htm",
    
    init: function (data) {
        this.saveLink = data.save;
        this.http = http;
        
        this.date = ko.observable(moment().sod().format("LL"));
        this.odometer = ko.observable();
        this.pricePerUnit = ko.observable();
        this.totalUnits = ko.observable();
        this.vendor = ko.observable();
        this.transactionFee = ko.observable();
        this.remarks = ko.observable();

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

        this.totalCost = ko.computed(function() {
            var total = parseFloat(this.pricePerUnit()) * parseFloat(this.totalUnits()) + parseFloat(this.transactionFee());
            return isNaN(total) ? "" : total.toFixed(2);
        }, this);
    },
    
    save: function () {
        if (!this.validate()) return;

        var data = {
            date: moment(this.date()).format("YYYY-MM-DD"),
            odometer: parseInt(this.odometer()),
            pricePerUnit: parseFloat(this.pricePerUnit()),
            totalUnits: parseInt(this.totalUnits(), 10),
            transactionFee: parseFloat(this.transactionFee()),
            remarks: this.remarks(),
            vendor: this.vendor()
        };

        this.http(this.saveLink, data);
    },
    
    validate: function () {
        var allValid = true;
        for (var propertyName in this) {
            if (this[propertyName].validation) {
                if (!this[propertyName].validation.validate()) {
                    allValid = false;
                }
            }
        }
        return allValid;
    }
    
});