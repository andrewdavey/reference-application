/// <reference path="../convert.js"/>

converters["integer"] = {
    fromString: function (s) {
        var i = parseInt(s, 10);
        return { error: isNaN(i), value: i };
    },
    toString: function (i) {
        return typeof i === "number" ? i.toString() : "";
    }
}