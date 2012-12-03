/// <reference path="ProfilePage.js"/>
/// <reference path="../Shared/http.js"/>

var init = function (viewData) {
    return ProfilePage.create(viewData, http);
};