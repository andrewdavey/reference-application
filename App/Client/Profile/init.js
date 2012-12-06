/// <reference path="ProfilePage.js"/>
/// <reference path="../Shared/http.js"/>

var init = function (viewData, app) {
    return ProfilePage.create(viewData, app, http);
};