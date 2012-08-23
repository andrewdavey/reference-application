/// <reference path="DashboardViewModel.js"/>

var init = function (pageData, app) {
    return DashboardViewModel.create(pageData, app.flashMessage, app.eventHub);
};