/// <reference path="~/Client/Shared/Base.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="../Profile/ProfileForm.js" />
/// <reference path="../Vehicles/List/VehicleSummaryList.js" />

var Dashboard = Base.inherit({

    templateId: "Client/Dashboard/dashboard.htm",

    init: function (pageData, flashMessage, eventHub, http) {
        this.http = http;
        this.flashMessage = flashMessage;
        this.initStatistics(pageData);
        this.initVehicles(pageData, eventHub, http);
        this.initReminders(pageData);
        this.initProfile(pageData);
    },
    
    initStatistics: function(pageData) {
        this.statistics = pageData.statistics;
    },
    
    initVehicles: function(pageData, eventHub, http) {
        this.vehicles = VehicleSummaryList.create(pageData.vehicles, eventHub, http);
        this.addVehicleUrl = pageData.addVehicle.url;
    },
    
    initReminders: function(pageData) {
        this.reminders = pageData.reminders;
    },
    
    initProfile: function (pageData) {
        this.profile = ko.observable();
        
        this.http(pageData.profile)
            .then(this.displayProfileFormIfProfileIncomplete.bind(this));
    },
    
    displayProfileFormIfProfileIncomplete: function (profileData) {
        if (profileData.hasRegistered) return;
        
        var profileForm = ProfileForm.create(profileData, this.http);
        profileForm.onSaved.subscribe(this.hideProfile.bind(this));
        this.profile(profileForm);
    },
    
    hideProfile: function () {
        this.flashMessage.show("Profile saved");
        this.profile(null);
    }
    
});