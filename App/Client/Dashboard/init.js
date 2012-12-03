/// <reference path="Dashboard.js"/>
/// <reference path="../Shared/flashMessage.js"/>
/// <reference path="../Shared/eventHub.js"/>
/// <reference path="../Shared/http.js"/>

// Entry point for the Dashboard page.
var init = function (pageData) {
    return Dashboard.create(pageData, flashMessage, eventHub, http);
};