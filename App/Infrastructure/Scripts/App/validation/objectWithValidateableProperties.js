var objectWithValidateableProperties = {
    validate: function() {
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
};