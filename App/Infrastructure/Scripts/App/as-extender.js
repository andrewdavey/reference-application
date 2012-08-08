/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>

ko.extenders["as"] = function (originalObservable, options) {
    return ko.extenders["as"][options](originalObservable);
};

ko.extenders["as"]["integer"] = function (originalObservable) {
    // Usage: ko.observable().extend({ as: "integer" })
    //
    // The original observable store a JavaScript Number.
    // A `asString` observable property is added to the original observable.
    // The `asString` observable returns the original number as a string.
    // A string written to `asString` is parsed into an integer and written to the original observable.

    // When an invalid string (i.e. won't parse as an integer) is passed in from the UI
    // we need to store it to remain consistent.
    var invalidStringValue = ko.observable();

    originalObservable.asString = ko.computed({
        read: function () {
            return (invalidStringValue() || originalObservable() || "").toString();
        },
        write: function (stringValue) {
            var intValue = parseInt(stringValue, 10);
            if (isNaN(intValue)) {
                originalObservable(null);
                invalidStringValue(stringValue);
            } else {
                originalObservable(intValue);
                invalidStringValue(null);
            }
        }
    });

    // If there was an invalid string, but later the original observable is updated,
    // then clear the invalid string. This will result in the `read` function being called
    // so the now valid string is displayed.
    originalObservable.subscribe(function() {
        invalidStringValue(null);
    });

    return originalObservable;
};

ko.extenders["as"]["money"] = function(originalObservable) {
    var invalidStringValue = ko.observable();

    originalObservable.asString = ko.computed({
        read: function () {
            if (invalidStringValue()) {
                return invalidStringValue();
            } else if (originalObservable()) {
                return originalObservable().toFixed(2);
            } else {
                return "";
            }
        },
        write: function (stringValue) {
            var floatValue = parseFloat(stringValue);
            if (isNaN(floatValue)) {
                originalObservable(null);
                invalidStringValue(stringValue);
            } else {
                originalObservable(Math.round(floatValue * 100) / 100);
                invalidStringValue(null);
            }
        }
    });

    originalObservable.subscribe(function () {
        invalidStringValue(null);
    });

    return originalObservable;
};