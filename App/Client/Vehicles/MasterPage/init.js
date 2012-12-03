/// <reference path="MasterPage.js" />
/// <reference path="../../Shared/eventHub.js"/>
/// <reference path="../../Shared/http.js"/>

var init = function (viewData) {
    return MasterPage.create(viewData, eventHub, http);
};