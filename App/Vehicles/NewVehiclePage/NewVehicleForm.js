/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>

var NewVehicleForm = Object.inherit({

    templateId: "Vehicles/NewVehiclePage/NewVehicleForm.htm",

    init: function (pageData, app) {
        this.app = app;
        this.http = http;
        this.saveCommand = pageData.save;

        this.initInputProperties();
        this.initReferenceData(pageData);
        this.initValidation();
    },
    
    initInputProperties: function () {
        // Input properties are bound to <form> inputs in the UI.
        // They collect the user input so we can save it later.
        this.name = ko.observable();
        this.year = ko.observable(); // year info object i.e. { year: <number>, makes: { get: <URL-for-the-makes> } }
        this.make = ko.observable(); // make info object i.e. { make: <string>, models: { get: <URL-for-the-models> } }
        this.model = ko.observable(); // model name string
    },
    
    initReferenceData: function(pageData) {
        // Reference data for <select> inputs
        this.years = ko.observableArray(); // array of year info object
        this.makes = ko.observableArray(); // array of make info objects
        this.models = ko.observableArray(); // array of model strings
        
        this.subscribeToReferenceData(pageData);
    },
    
    initValidation: function () {
        this.name.validation = {
            message: ko.observable()
        };
    },
    
    subscribeToReferenceData: function (pageData) {
        // When a year is selected, download the makes.
        this.year.subscribe(this.downloadMakes, this);
        // When a make is selected, download the models.
        this.make.subscribe(this.downloadModels, this);

        // Download the years now.
        this.http(pageData.years)
            .done(this.displayYears);
    },
    
    downloadMakes: function (year) {
        if (year) {
            this.http(year.makes)
                .done(this.displayMakes);
        } else {
            this.displayMakes([]);
            this.make(null);
        }
    },
    
    downloadModels: function (make) {
        if (make) {
            this.http(make.models)
                .done(this.displayModels);
        } else {
            this.displayModels([]);
            this.model(null);
        }
    },
    
    displayYears: function (years) {
        this.years(years);
    },
    
    displayMakes: function (makes) {
        this.makes(makes);
    },
    
    displayModels: function (models) {
        this.models(models);
    },

    save: function () {
        if (!this.validate()) return;
        this.http(this.saveCommand, this.serializeForm())
            .done(this.saved);
    },
    
    validate: function() {
        if (!this.name()) {
            this.name.validation.message("Name is required");
            return false;
        }
        return true;
    },

    serializeForm: function () {
        return {
            Name: this.name(),
            Year: this.year() ? parseInt(this.year().year, 10) : null,
            MakeName: this.make() ? this.make().make : null,
            ModelName: this.model()
        };
    },

    saved: function (data, status, xhr) {
        var vehicleUrl = xhr.getResponseHeader("Location");
        this.app.navigate(vehicleUrl);
    }
});