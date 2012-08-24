﻿/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/EventHub.js"/>
/// <reference path="~/Client/Shared/FlashMessage.js"/>

var AppFrame = Object.inherit({
    
    init: function (viewData) {
        this.links = viewData.links;
        this.flashMessage = flashMessage;
        this.content = ko.observable();
    },
    
    templateId: "Client/AppFrame/AppFrame.htm"
    
});