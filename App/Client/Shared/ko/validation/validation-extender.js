/// <reference path="../../../Vendor/knockout.js"/>

// The validation extender adds validation to an observable.
//
// Usage:
//     myObservable.extend({ validation: { required: "This is required" } });
//
// See validators.js for implemented validators e.g. required
//
// A validation property is added to the target observable. It has the form:
//     {
//         message:   <observable-string>, // The current error message, or empty string if valid.
//         validate:  <function>,          // Function that runs all validations against the current value. Returns true if all are valid.
//         isInvalid: <observable-boolean> // True when any validations are invalid for the current value.
//     }
//
// Changing value of target will trigger validation. Or validate() can be called, which returns true if valid.

ko.extenders.validation = function (target, options) {
    
    var validationFunctions = Object
        .keys(options)
        .map(function (key) {
            return function(value) {
                return ko.extenders.validation.validators[key](value, options[key]);
            };
        });
    
    var validatorResultIsError = function(result) {
        return typeof result === "string";
    };

    var getErrors = function(value) {
        return validationFunctions
            .map(function (validationFunction) { return validationFunction(value); })
            .filter(validatorResultIsError);
    };
    
    target.validation = {
        message: ko.observable(""),
        validate: function () {
            var value = target();
            var errors = getErrors(value);
            if (errors.length === 0) {
                target.validation.message("");
                return true;
            } else {
                target.validation.message(errors[0]);
                return false;
            }
        }
    };

    // Convenience property to simplify UI bindings.
    target.validation.isInvalid = ko.computed(function() {
        return !!target.validation.message();
    });
    
    // When the target observable changes, run the validation.
    target.subscribe(function () {
        target.validation.validate();
    });

    return target;
};