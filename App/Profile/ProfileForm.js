/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/httpCommand.js"/>

var ProfileForm = Object.inherit({
    
    init: function (data) {
        this.initInputs();
        this.initCountries(data.countries);
        this.initValidation();
        this.saveCommand = data.save;
    },
    
    initInputs: function () {
        this.name = ko.observable();
        this.country = ko.observable();
    },
    
    initCountries: function (getCountries) {
        this.countries = ko.observableArray();

        getCountries(this).done(function (countriesResponse) {
            this.countries(countriesResponse.countries);
        });
    },
    
    initValidation: function () {
        this.name.validation = {
            message: ko.observable()
        };
    },
    
    save: function () {
        if (!this.validate()) return;
        
        var profileData = {
            name: this.name(),
            country: this.country()
        };
        this.saveCommand(profileData)
            .done(this.saved);
    },
    
    saved: function () {
    },
    
    validate: function () {
        if (!this.name()) {
            this.name.validation.message("Name is required");
            return false;
        }
        return true;
    }
});