/// <reference path="../../Vendor/knockout.js"/>

var converters = {}; // Converters are added to this object in ./converters/*.js

(function () {

    // A convertingObservable wraps an underlying observable.
    // It provides a string representation of the underlying observable.
    // A converter object is used to convert to and from strings.
    var convertingObservable = function(underlying, converter) {
        var invalidString = ko.observable();
        var updating = false;
        
        underlying.subscribe(function () {
            if (updating) return;
            // The underlying observable was changed by something other than us.
            // So clear any previous invalid string.
            // This will trigger the `read` funtion below.
            invalidString(null);
        });
        
        return ko.computed({
            
            read: function() {
                var value = underlying();
                var invalid = invalidString();
                return typeof invalid === "string" ? invalid : converter.toString(value);
            },
            
            write: function(str) {
                var result = converter.fromString(str);
                if (result.error) {
                    invalidString(str);
                } else {
                    invalidString(null);
                }
                updating = true;
                underlying(result.value);
                updating = false;
                
                // Converting "10.5" to "integer" and then "10.7" to "integer"
                // results in no real change to the underlying, or the 
                // invalidString observable. However, we do want to re-read
                // the converted observable so it can format "10.7" into "10".
                // So force the `read`:
                invalidString.valueHasMutated();
            }
            
        });
    };

    // Extend knockout observables by adding a `convert` function.
    // Usage: <input type="text" data-bind="value: payment.convert('money')" />
    ko.observable.fn["convert"] = function (converterName) {
        if (this.__converted__) return this.__converted__;
        
        var converter = converters[converterName];
        if (!converter) throw "Cannot find converter: " + converterName;

        // `this` is the observable object we need to wrap a converter around.
        return (this.__converted__ = convertingObservable(this, converter));
    };
}());