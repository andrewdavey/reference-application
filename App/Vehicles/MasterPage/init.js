/// <reference path="MasterPage.js" />

var init = function (viewData, app) {
    return MasterPage.create(viewData, app.eventHub);
};