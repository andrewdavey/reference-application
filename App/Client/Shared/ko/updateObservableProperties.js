/// <reference path="../../Vendor/knockout.js"/>

var updateObservableProperties = function (source, target) {
    var updateProperty = function(propertyName) {
        var property = target[propertyName];
        if (ko.isObservable(property)) {
            property(source[propertyName]);
        }
    };
    Object
        .keys(source)
        .forEach(updateProperty);
};
