/// <reference path="../converters.js"/>

// The integer converter displays integer numbers as strings.
// Any fractional values are discarded when converting strings into numbers.
// e.g. 10     -> "10"
//      "10.5" -> 10

converters["integer"] = {
    
    fromString: function (string) {
        var value = parseInt(string, 10);
        return {
            error: isNaN(value),
            value: value
        };
    },
    
    toString: function (value) {
        return typeof value === "number" ? value.toString() : "";
    }
    
}