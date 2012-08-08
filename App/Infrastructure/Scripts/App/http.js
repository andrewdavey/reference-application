/// <reference path="../Vendor/jquery.js"/>
/// <reference path="IframeSubmission.js"/>

var http = function (action, data, files) {
    if (files) {
        var submission = IframeSubmission.create(action.method, action.url, data, files, this);
        return submission.submit();
    } else {
        var request = $.ajax({
            type: action.method,
            url: action.url,
            data: data,
            // Assign context for chained callbacks.
            context: this,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("Accept", "application/json");
            }
        });

        var doneFilter = null; // Do not transform the `done` result.
        
        var failFilter = function (xhr) {
            var isValidationErrorJson = xhr.getResponseHeader("Content-Type") === "application/x-validation-errors+json";
            if (isValidationErrorJson) {
                try {
                    var errors = JSON.parse(xhr.responseText);
                    return { validationErrors: errors };
                } catch (e) {
                    return { validationErrors: { "": "Server returned invalid data." } };
                }
            } else {
                return xhr;
            }
        };
        
        request = request.pipe(doneFilter, failFilter);
        
        return request;
    }
};