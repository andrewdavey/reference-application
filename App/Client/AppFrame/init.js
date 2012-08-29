/// <reference path="AppFrame.js"/>
/// <reference path="~/Client/Shared/FlashMessage.js"/>

// `init` is called by the application infrastructure when this module
// has been downloaded. It creates the view model for this module.
var init = function (viewData) {
    return AppFrame.create(viewData, flashMessage);
};