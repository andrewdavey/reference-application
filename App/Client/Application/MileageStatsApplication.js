/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/Application.js"/>
/// <reference path="~/Client/Shared/EventHub.js"/>
/// <reference path="~/Client/Shared/FlashMessage.js"/>

var MileageStatsApplication = Application.inherit({
    
    init: function () {
        Application.init.apply(this, arguments);
        this.flashMessage = FlashMessage.create();
        this.eventHub = EventHub.create();
    },
    
    templateId: "Client/Application/MileageStatsApplication.htm"
    
});