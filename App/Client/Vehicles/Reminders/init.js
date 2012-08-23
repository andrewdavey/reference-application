/// <reference path="RemindersPage.js" />

var init = function (viewData, app) {
    return RemindersPage.create(viewData, app.flashMessage);
};