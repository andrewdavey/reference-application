/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/Object.js"/>
/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Shared/validation/objectWithValidateableProperties.js" />

var ProfileForm = Object.inherit({
    
    init: function (data) {
        this.http = http;
        this.initInputs();
        this.initCountries(data.countries);
        this.initValidation();
        this.saveCommand = data.save;
    },
    
    initInputs: function () {
        this.name = ko.observable();
        this.country = ko.observable();
    },
    
    initCountries: function (countriesLink) {
        this.countries = ko.observableArray();

        this.http(countriesLink)
            .done(function (countriesResponse) {
                this.countries(countriesResponse.countries);
            });
    },
    
    initValidation: function () {
        this.validate = objectWithValidateableProperties.validate;
        this.name.extend({
            validation: {
                required: "Name is required"
            }
        });
    },
    
    save: function () {
        if (!this.validate()) return;
        
        var profileData = {
            name: this.name(),
            country: this.country()
        };
        this.http(this.saveCommand, profileData)
            .done(this.saved);
    },
    
    saved: function () {
    }
});