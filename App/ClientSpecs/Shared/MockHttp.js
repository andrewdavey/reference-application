/// <reference path="~/Client/Shared/Base.js"/>

var MockHttp = Base.inherit({
    init: function() {
        this.http = this.http.bind(this);
        this.reset();
    },
    
    reset: function() {
        this.recordedActions = [];
        this.requested = [];
    },
    
    http: function (action) {
        var matches = this.recordedActions.filter(function (recordedAction) {
            return recordedAction.method === action.method
                && recordedAction.url === action.url;
        });
        if (matches.length > 0) {
            if (matches[0].callback) {
                matches[0].callback.apply(this, arguments);
            }
            var response = matches[0].response;
            this.requested.push(action);
            return $.Deferred().resolveWith(this, [response]);
        }

        throw "Mock HTTP not configured for: " + action.method + " " + action.url;
    },
    
    wasRequestMade: function(method, url) {
        return this
            .requested
            .filter(function(r) { return r.method === method && r.url === url; })
            .length > 0;
    }
});

// Add helper methods to MockHttp
["get", "put", "post", "delete"].forEach(function(httpMethod) {
    MockHttp[httpMethod] = function (url) {
        var action = { method: httpMethod, url: url };
        this.recordedActions.push(action);
        return {
            calls: function (callback) {
                action.callback = callback;
            },
            respondsWith: function (response) {
                action.response = response;
            }
        };
    };
});