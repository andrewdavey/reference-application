/// <reference path="MasterPage.js" />
/// <reference path="../../Shared/EventHub.js"/>

var init = function (viewData) {
    return MasterPage.create(viewData, eventHub);
};