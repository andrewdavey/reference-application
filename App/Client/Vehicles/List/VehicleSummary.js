/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/Base.js"/>
/// <reference path="~/Client/Shared/ko/updateObservableProperties.js"/>

var VehicleSummary = Base.inherit({
    init: function(data) {
        this.name = ko.observable(data.name);
        this.photo = ko.observable(data.photo ? data.photo.url : "");
        this.year = ko.observable(data.year ? data.year : "");
        this.make = ko.observable(data.make ? data.make : "");
        this.model = ko.observable(data.model ? data.model : "");
        this.averageFuelEfficiency = data.averageFuelEfficiency;
        this.averageCostToDrive = data.averageCostToDrive;
        this.averageCostPerMonth = data.averageCostPerMonth;
        this.details = data.details.url;
        this.fillUps = data.fillUps.url;
        this.reminders = data.reminders.url;
    },

    update: function (data) {
        updateObservableProperties(data, this);
        this.forcePhotoUpdate();
    },

    forcePhotoUpdate: function () {
        var newUrl = this.appendCacheBreakerToUrl(this.photo());
        this.photo(newUrl);
    },

    appendCacheBreakerToUrl: function (url) {
        var questionMarkIndex = url.indexOf("?");
        var cacheBreaker = (new Date()).getTime().toString();
        if (questionMarkIndex < 0) {
            return url + "?" + cacheBreaker;
        } else {
            return url.substr(0, questionMarkIndex + 1) + cacheBreaker;
        }
    }
});