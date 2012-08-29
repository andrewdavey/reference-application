/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/Base.js"/>
/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Shared/validation/objectWithValidateableProperties.js" />
/// <reference path="~/Client/Shared/Event.js"/>

var ProfileForm = Base.inherit({
    
    init: function (data) {
        this.http = http;
        this.initInputs(data);
        this.initCountries(data.countries);
        this.initValidation();
        this.saveCommand = data.save;
        this.onSaved = Event.create();
    },
    
    initInputs: function (data) {
        this.name = ko.observable(data.name);
        this.country = ko.observable(data.country);
    },
    
    initCountries: function (countriesLink) {
        this.countries = ko.observableArray();

        // The list of countries is loaded asynchronously.
        // This means the view's <select> is initially empty,
        // which can reset this.country() to be null.
        // Therefore, if we have a country already, temporarily 
        // make the array contain it.
        if (this.country()) this.countries([this.country()]);

        this.http(countriesLink)
            .done(function (countriesResponse) {
                this.countries(countriesResponse.countries);
            });
    },
    
    initValidation: function () {
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
        this.onSaved.trigger();
    }
}).mixin(objectWithValidateableProperties);