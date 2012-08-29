/// <reference path="~/Client/Shared/Base.js"/>
/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Shared/Modal.js"/>
/// <reference path="~/Client/Shared/ko/validation/objectWithValidateableProperties.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>

var EditVehicleForm = Base.inherit({

    templateId: "Client/Vehicles/Details/EditVehicleForm.htm",

    init: function (viewData, flashMessage) {
        this.viewData = viewData;
        this.flashMessage = flashMessage;
        this.http = http;
        this.saveCommand = viewData.save;

        this.initInputProperties(viewData);
        this.initReferenceData(viewData);
        this.initValidation();
    },
    
    initInputProperties: function (viewData) {
        // Input properties are bound to <form> inputs in the UI.
        // They collect the user input so we can save it later.
        this.name = ko.observable(viewData.name);
        this.year = ko.observable(); // year info object i.e. { year: <number>, makes: { get: <URL-for-the-makes> } }
        this.make = ko.observable(); // make info object i.e. { make: <string>, models: { get: <URL-for-the-models> } }
        this.model = ko.observable(); // model name string
        this.photo = ko.observable();
    },
    
    initReferenceData: function(pageData) {
        // Reference data for <select> inputs
        this.years = ko.observableArray(); // array of year info object
        this.makes = ko.observableArray(); // array of make info objects
        this.models = ko.observableArray(); // array of model strings
        
        this.subscribeToReferenceData(pageData);
    },
    
    initValidation: function () {
        this.name.extend({
            validation: {
                required: "Name is required"
            }
        });
    },
    
    show: function () {
        this.modal = Modal.create(this);
        return this.modal.showing;
    },
    
    close: function (vehicleData) {
        this.modal.closeWithResult(vehicleData);
    },
    
    subscribeToReferenceData: function (viewData) {
        // When a year is selected, download the makes.
        this.year.subscribe(this.downloadMakes, this);
        // When a make is selected, download the models.
        this.make.subscribe(this.downloadModels, this);

        // Download the years now.
        this.http(viewData.years)
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
        var currentYear = (this.year() && this.year().year) || this.viewData.year;
        
        this.years(years);
        
        var yearViewModel = years.filter(function(y) { return y.year === currentYear; })[0];
        this.year(yearViewModel);
    },
    
    displayMakes: function (makes) {
        var currentMake = (this.make() && this.make().make) || this.viewData.make;
        
        this.makes(makes);

        var makeViewModel = makes.filter(function (m) { return m.make === currentMake; })[0];
        this.make(makeViewModel);
    },
    
    displayModels: function (models) {
        var currentModel = this.model() || this.viewData.model;
        
        this.models(models);

        this.model(currentModel);
    },

    save: function () {
        if (!this.validate()) return;
        var vehicleData = this.serializeForm();
        var files = [ this.photo() ];
        this.http(this.saveCommand, vehicleData, files)
            .done(function () {
                var args = Array.prototype.slice.call(arguments);
                args[0] = vehicleData;
                this.saved.apply(this, args);
            });
    },
    
    serializeForm: function () {
        return {
            href: this.saveCommand.url,
            name: this.name(),
            year: this.year() ? parseInt(this.year().year, 10) : null,
            make: this.make() ? this.make().make : null,
            model: this.model()
        };
    },

    saved: function (vehicleData) {
        this.flashMessage.show("Saved");
        this.close(vehicleData);
    },
    
    cancel: function () {
        this.modal.close();
    }
}).mixin(objectWithValidateableProperties);