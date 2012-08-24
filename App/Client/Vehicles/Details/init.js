/// <reference path="Vehicle.js"/>
/// <reference path="../../Shared/EventHub.js"/>
/// <reference path="../../Shared/FlashMessage.js"/>

var init = function (pageData, app) {
    return Vehicle.create(pageData, app, eventHub, flashMessage);
};