﻿/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/moment.js"/>
/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/App/validation/objectWithValidateableProperties.js"/>
/// <reference path="~/Infrastructure/Scripts/App/popups.js" />

var AddReminderForm = Object.inherit({
    init: function (viewData) {
        this.http = http;
        this.addCommand = viewData.add;
        this.initInputs();
        this.initValidation();
    },
    
    templateId: "Vehicles/Reminders/AddReminderForm.htm",
    
    initInputs: function () {
        this.title = ko.observable();
        this.remarks = ko.observable();
        this.dueDate = ko.observable();
        this.dueDistance = ko.observable().extend({ as: "integer" });
    },
    
    initValidation: function () {
        this.validate = objectWithValidateableProperties.validate;
        
        this.title.extend({
            validation: { required: "Title is required" }
        });
        this.dueDistance.extend({
            validation: {}
        });
    },
    
    show: function () {
        this.closed = $.Deferred();
        popups.modal(this);
        return this.closed;
    },
    
    close: function (reminderData) {
        popups.closeModal(this);
        this.closed.resolve(reminderData);
    },
    
    submit: function () {
        if (!this.validate()) return;
        var formData = this.formData();
        this.http(this.addCommand, formData)
            .pipe(this.getCreatedReminder)
            .done(this.close)
            .fail(this.submitFailed);
    },
    
    getCreatedReminder: function (response, status, xhr) {
        var reminderUrl = xhr.getResponseHeader("Location");
        return this.http({ method: "get", url: reminderUrl });
    },
    
    formData: function () {
        var dueDistance = this.dueDistance();
        var dueDate = this.dueDate();
        var data = {
            title: this.title(),
            remarks: this.remarks()
        };
        if (dueDate) data.dueDate = moment(dueDate).format("YYYY-MM-DD");
        if (dueDistance) data.dueDistance = dueDistance;
        return data;
    },
    
    cancel: function () {
        this.close();
    },

    submitFailed: function (response) {
        if (response.validationErrors) {
            Object.keys(response.validationErrors).forEach(function (key) {
                var property = this[key];
                if (property && property.validation) {
                    property.validation.message(response.validationErrors[key].join(". "));
                }
            }, this);
        } else {
            alert("There was a problem adding the reminder. Please try again.");
        }
    }
});