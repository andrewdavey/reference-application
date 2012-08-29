/// <reference path="../convert.js"/>

// The money converter displays numbers as strings rounded to 2 decimal places.
// e.g. 10.5 -> "10.50"

converters["money"] = {
    
    fromString: function(string) {
        var value = parseFloat(string, 10);
        var roundedValue = Math.round(100 * value) / 100;
        return {
            error: isNaN(roundedValue),
            value: roundedValue
        };
    },

    toString: function(value) {
        return typeof value === "number" ? value.toFixed(2) : "";
    }

};