/// <reference path="Vehicle.js"/>
/// <reference path="../../Shared/eventHub.js"/>
/// <reference path="../../Shared/flashMessage.js"/>
/// <reference path="../../Shared/http.js"/>

var init = function (pageData, app) {
    return Vehicle.create(pageData, app, eventHub, flashMessage, http);
};