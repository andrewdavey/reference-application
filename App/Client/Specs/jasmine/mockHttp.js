/// <reference path="../jasmine/jasmine.js"/>
/// <reference path="../jasmine/matchers.js"/>

var mockHttp = function (action) {
    var matches = mockHttp.recordedActions.filter(function (recordedAction) {
        return recordedAction.method === action.method
            && recordedAction.url === action.url;
    });
    if (matches.length > 0) {
        if (matches[0].callback) {
            matches[0].callback.apply(this, arguments);
        }
        var response = matches[0].response;
        mockHttp.requested.push(action);
        return $.Deferred().resolveWith(this, [response]);
    }
    
    throw "Mock HTTP not configured for: " + action.method + " " + action.url;
};

mockHttp.recordedActions = [];
mockHttp.requested = [];

mockHttp.reset = function() {
    mockHttp.recordedActions = [];
    mockHttp.requested = [];
};

function defineHelper(httpMethod) {
    mockHttp[httpMethod] = function(url) {
        var action = { method: httpMethod, url: url };
        mockHttp.recordedActions.push(action);
        return {
            calls: function(callback) {
                action.callback = callback;
            },
            respondsWith: function(response) {
                action.response = response;
            }
        };
    };
}

defineHelper("get");
defineHelper("put");
defineHelper("post");
defineHelper("delete");

// Replace the real `http` function with the mock.
require(["Client/Shared"], function (app) {
    app.http = mockHttp;
});

beforeEach(function() {
    this.addMatchers({
        requested: function(method, url) {
            return mockHttp.requested
                .filter(function(r) { return r.method === method && r.url === url; })
                .length;
        }
    });
});