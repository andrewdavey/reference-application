/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/EventHub.js"/>
/// <reference path="~/Client/Shared/FlashMessage.js"/>

var AppFrame = Base.inherit({
    
    init: function (viewData) {
        this.links = viewData.links;
        this.links.forEach(function (link) { link.rel = link.rel || null; });
        this.flashMessage = flashMessage;
        this.content = ko.observable();
    },
    
    templateId: "Client/AppFrame/AppFrame.htm"
    
});