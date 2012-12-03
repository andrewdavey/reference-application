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

    var appendContentTypeToQueryString = function (url, contentType) {
        // Chrome doesn't obey the HTTP Vary header when using the back button.
        // http://code.google.com/p/chromium/issues/detail?id=94369
        // So we have to differentiate requests by URL.
        // Otherwise, application/json responses can be displayed in place of text/html!
        return url + (url.indexOf("?") < 0 ? "?" : "&") + contentType;
    };
    
    var ajaxRequest = function (action, data) {
        var url = appendContentTypeToQueryString(action.url, "application/json");
        
        var request = $.ajax({
            // Assign context for chained callbacks.
            context: this,
            type: action.method,
            url: url,
            // Send data as JSON.
            data: JSON.stringify(data, replaceJsonValues),
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Content-Type", "application/json");
                xhr.setRequestHeader("Accept", action.accept || "application/json");
            }
        });

        // Pipe the request response through filters that may transform the response.
        request = request.pipe(doneFilter, failFilter);

        return request;
    };

    var getAction = function (object) {
        if (object.method && object.url) return object;
        
        var httpMethod = Object.keys(object)[0];
        return { method: httpMethod, url: object[httpMethod] };
    };
    
    return function (actionObject, data, files) {
        // actionObject has the format { <http-method>: <url-string> }
        // e.g. { get: "/resource" }
        
        var action = getAction(actionObject);
        if (files) {
            return IframeSubmission.create(action.method, action.url, data, files, this).request;
        } else {
            return ajaxRequest.call(this, action, data);
        }
    };
}());