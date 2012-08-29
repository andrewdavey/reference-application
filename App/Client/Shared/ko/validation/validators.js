/// <reference path="../../../Vendor/knockout.js"/>
/// <reference path="validation-extender.js" />

ko.extenders.validation.validators = {
    
    required: function (value, message) {
        return value ? true : message;
    },
    
    pattern: function (value, options) {
        return options.regex.test(value) ? true : options.message;
    },
    
    money: function (value) {
        return ko.extenders.validation.validators.pattern(
            value,
            {
                regex: /^(\d*\.\d\d?|\d+)$/,
                message: "Please enter an amount of money"
            }
        );
    },
    
    greaterThan: function (value, min) {
        return value > min ? true : ("Must be greater than " + min);
    }
    
};