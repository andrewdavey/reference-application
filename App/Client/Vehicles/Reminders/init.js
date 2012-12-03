/// <reference path="RemindersPage.js" />
/// <reference path="../../Shared/flashMessage.js"/>
/// <reference path="../../Shared/http.js"/>

var init = function (viewData) {
    return RemindersPage.create(viewData, flashMessage, http);
};