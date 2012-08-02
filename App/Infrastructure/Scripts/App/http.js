/// <reference path="../Vendor/jquery.js"/>
/// <reference path="IframeSubmission.js"/>

var http = function (action, data, files) {
    if (files) {
        var submission = IframeSubmission.create(action.method, action.url, data, files, this);
        return submission.submit();
    } else {
        return $.ajax({
            type: action.method,
            url: action.url,
            data: data,
            // Assign context for chained callbacks.
            context: this
        });
    }
};