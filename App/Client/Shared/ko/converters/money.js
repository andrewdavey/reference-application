/// <reference path="../convert.js"/>

converters["money"] = {
    fromString: function(s) {
        var i = Math.round(100 * parseFloat(s, 10)) / 100;
        return { error: isNaN(i), value: i };
    },
    toString: function(i) {
        return typeof i === "number" ? i.toFixed(2) : "";
    }
};