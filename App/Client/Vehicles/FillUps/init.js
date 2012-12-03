/// <reference path="FillUpsPage.js" />
/// <reference path="../../Shared/flashMessage.js"/>

var init = function (pageData) {
    return FillUpsPage.create(pageData, flashMessage);
};