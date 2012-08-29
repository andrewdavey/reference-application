/// <reference path="../../Vendor/knockout.js"/>

ko.bindingHandlers["file"] = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        value(element);
    }
};