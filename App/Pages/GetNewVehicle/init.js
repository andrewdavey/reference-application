/// <reference path="NewVehicleForm.js"/>

var init = function (pageData, app) {
    app.setViewModel(NewVehicleForm.create(pageData, app));
};