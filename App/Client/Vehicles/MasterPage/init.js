/// <reference path="MasterPage.js" />
/// <reference path="../../Shared/eventHub.js"/>

var init = function (viewData) {
    return MasterPage.create(viewData, eventHub);
};