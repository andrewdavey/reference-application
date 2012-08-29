/// <reference path="Dashboard.js"/>
/// <reference path="../Shared/FlashMessage.js"/>
/// <reference path="../Shared/EventHub.js"/>

// `init` is called by the application infrastructure when this module
// has been downloaded. It creates the view model for this module.
var init = function (pageData) {
    return Dashboard.create(pageData, flashMessage, eventHub);
};