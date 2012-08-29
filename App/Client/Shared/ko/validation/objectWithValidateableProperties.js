var objectWithValidateableProperties = {
    validate: function () {
        // Returns true if all properties of `this`, which have validation, are valid.
        
        var getProperty = function(key) { return this[key]; };
        var hasValidation = function (property) { return property && property.validation && property.validation.validate; };
        var callValidate = function (property) { return property.validation.validate(); };
        var isNotValid = function (result) { return result !== true; };
        
        var errorCount = Object
            .keys(this)
            .map(getProperty, this)
            .filter(hasValidation)
            .map(callValidate)
            .filter(isNotValid)
            .length;

        return errorCount === 0;
    }
};