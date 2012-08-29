var Base;

(function () {
    "use strict";
    
    var copyOwnProperties = function (from, to) {
        for (var propertyName in from) {
            if (from.hasOwnProperty(propertyName)) {
                to[propertyName] = from[propertyName];
            }
        }
    };

    Base = {};
    Base.inherit = function (additionalProperties) {
        var prototype = Object.create(this);

        prototype.create = function () {
            var instance = Object.create(prototype);
            if (typeof instance.init === "function") {
                instance.init.apply(instance, arguments);
            }
            return instance;
        };

        copyOwnProperties(additionalProperties, prototype);

        return prototype;
    };
} ());