/// <reference path="RemindersPage.js" />
/// <reference path="../../Shared/flashMessage.js"/>

var init = function (viewData) {
    return RemindersPage.create(viewData, flashMessage);
};