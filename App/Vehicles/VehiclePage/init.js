/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="VehicleViewModel.js"/>

var init = function (pageData, app) {
    app.setViewModel(VehicleViewModel.create(pageData));
};