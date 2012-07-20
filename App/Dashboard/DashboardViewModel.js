/// <reference path="~/Infrastructure/Scripts/App/httpCommand.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="../Profile/ProfileForm.js" />
/// <reference path="../Vehicles/List/VehicleSummaryList.js" />

var DashboardViewModel = Object.inherit({

    templateId: "Dashboard/dashboard.htm",

    init: function (pageData, flashMessage) {
        this.flashMessage = flashMessage;
        this.initStatistics(pageData);
        this.initVehicles(pageData);
        this.initReminders(pageData);
        this.initProfile(pageData);
    },
    
    initStatistics: function(pageData) {
        this.statistics = pageData.statistics;
    },
    
    initVehicles: function(pageData) {
        this.vehicles = VehicleSummaryList.create(pageData.vehicles);
        this.addVehicleUrl = pageData.addVehicle;
    },
    
    initReminders: function(pageData) {
        this.reminders = pageData.reminders;
    },
    
    initProfile: function (pageData) {
        this.profile = ko.observable();
        
        var getProfile = httpCommand(pageData.profile, this);
        getProfile().then(this.displayProfileFormIfProfileIncomplete);
    },
    
    displayProfileFormIfProfileIncomplete: function (profileData) {
        if (profileData.name) return;
        
        var profileForm = ProfileForm.create(profileData);
        profileForm.saved = this.hideProfile.bind(this);
        this.profile(profileForm);
    },
    
    hideProfile: function () {
        this.flashMessage.show("Profile saved");
        this.profile(null);
    }
    
});