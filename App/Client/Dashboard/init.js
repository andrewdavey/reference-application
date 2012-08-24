/// <reference path="DashboardViewModel.js"/>
/// <reference path="../Shared/FlashMessage.js"/>
/// <reference path="../Shared/EventHub.js"/>

var init = function (pageData) {
    return DashboardViewModel.create(pageData, flashMessage, eventHub);
};