/// <reference path="RemindersPage.js" />
/// <reference path="../../Shared/FlashMessage.js"/>

var init = function (viewData) {
    return RemindersPage.create(viewData, flashMessage);
};