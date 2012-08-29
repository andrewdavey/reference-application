/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Shared/Base.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="../Profile/ProfileForm.js" />
/// <reference path="../Vehicles/List/VehicleSummaryList.js" />

var DashboardViewModel = Base.inherit({

    templateId: "Client/Dashboard/dashboard.htm",

    init: function (pageData, flashMessage, eventHub) {
        this.http = http;
        this.flashMessage = flashMessage;
        this.initStatistics(pageData);
        this.initVehicles(pageData, eventHub);
        this.initReminders(pageData);
        this.initProfile(pageData);
    },
    
    initStatistics: function(pageData) {
        this.statistics = pageData.statistics;
    },
    
    initVehicles: function(pageData, eventHub) {
        this.vehicles = VehicleSummaryList.create(pageData.vehicles, eventHub);
        this.addVehicleUrl = pageData.addVehicle.url;
    },
    
    initReminders: function(pageData) {
        this.reminders = pageData.reminders;
    },
    
    initProfile: function (pageData) {
        this.profile = ko.observable();
        
        this.http(pageData.profile)
            .then(this.displayProfileFormIfProfileIncomplete);
    },
    
    displayProfileFormIfProfileIncomplete: function (profileData) {
        if (profileData.hasRegistered) return;
        
        var profileForm = ProfileForm.create(profileData);
        profileForm.onSaved.subscribe(this.hideProfile.bind(this));
        this.profile(profileForm);
    },
    
    hideProfile: function () {
        this.flashMessage.show("Profile saved");
        this.profile(null);
    }
    
});