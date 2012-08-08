/// <reference path="../Vendor/jquery.js"/>
/// <reference path="../Vendor/moment.js"/>
/// <reference path="IframeSubmission.js"/>

var http = (function () {
    var toIso8601DateString = function (date) {
        return moment(date).format("YYYY-MM-DD");
    };

    var replaceJsonValues = function (key, value) {
        if (value instanceof Date) {
            // Convert Date object into ISO-8601 string.
            return toIso8601DateString(value);
        } else {
            return value;
        }
    };

    var doneFilter = null; // Do not transform the `done` result.
    var failFilter = function(xhr) {
        var isValidationErrorJson = xhr.getResponseHeader("Content-Type") === "application/x-validation-errors+json";
        if (isValidationErrorJson) {
            try {
                var errors = JSON.parse(xhr.responseText);
                return { validationErrors: errors };
            } catch(e) {
                return { validationErrors: { "": "Server returned invalid data." } };
            }
        } else {
            return xhr;
        }
    };

    var ajaxRequest = function(action, data) {
        var request = $.ajax({
            // Assign context for chained callbacks.
            context: this,
            type: action.method,
            url: action.url,
            // Send data as JSON.
            data: JSON.stringify(data, replaceJsonValues),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Content-Type", "application/json");
                // We want the server to send back responses as JSON.
                xhr.setRequestHeader("Accept", "application/json");
            }
        });

        // Pipe the request response through filters that may transform the response.
        request = request.pipe(doneFilter, failFilter);

        return request;
    };
    
    return function(action, data, files) {
        if (files) {
            return IframeSubmission.create(action.method, action.url, data, files, this).request;
        } else {
            return ajaxRequest(action, data);
        }
    };
}());