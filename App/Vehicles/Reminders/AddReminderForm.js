/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
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
        this.dueDistance = ko.observable();
    },
    
    initValidation: function () {
        this.validate = objectWithValidateableProperties.validate;
        
        this.title.extend({
            validation: { required: "Title is required" }
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
            .done(this.close);
    },
    
    getCreatedReminder: function (response, status, xhr) {
        var reminderUrl = xhr.getResponseHeader("Location");
        return this.http({ method: "get", url: reminderUrl });
    },
    
    formData: function () {
        return {
            title: this.title(),
            remarks: this.remarks(),
            dueDate: moment(this.dueDate()).format("YYYY-MM-DD"),
            dueDistance: parseInt(this.dueDistance(), 10)
        };
    },
    
    cancel: function () {
        this.close();
    }
});