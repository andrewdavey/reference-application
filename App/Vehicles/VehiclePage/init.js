/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="Page.js"/>

var init = function (pageData, app) {
    app.setViewModel(Page.create(pageData));
};