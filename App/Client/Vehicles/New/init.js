/// <reference path="NewVehicleForm.js"/>
/// <reference path="../../Shared/http.js"/>

var init = function (pageData, app) {
    return NewVehicleForm.create(pageData, app, http);
};