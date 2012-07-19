/// <reference path="DashboardViewModel.js"/>

var init = function (pageData, app) {
    app.setViewModel(DashboardViewModel.create(pageData, app.flashMessage));
};