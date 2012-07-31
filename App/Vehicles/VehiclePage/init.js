/// <reference path="Vehicle.js"/>

var init = function (pageData, app) {
    return Vehicle.create(pageData, app, app.eventHub, app.flashMessage);
};