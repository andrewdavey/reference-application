/// <reference path="~/Client/Shared/http.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Vendor/moment.js"/>
/// <reference path="~/Client/Shared/Base.js" />
/// <reference path="Reminder.js"/>
/// <reference path="AddReminderForm.js"/>

var RemindersPage = Base.inherit({
    init: function (viewData, flashMessage, http) {
        this.flashMessage = flashMessage;
        this.http = http;
        this.reminders = ko.observableArray(viewData.reminders.map(this.createReminderViewModel, this));
        this.selectedReminder = ko.observable(this.reminders()[0]);
        this.add = viewData.add;
    },
    
    templateId: "Client/Vehicles/Reminders/RemindersPage.htm",
    
    showAddReminderForm: function () {
        var form = AddReminderForm.create({ add: this.add }, this.http);
        form.show()
            .done(this.displayAddedReminder.bind(this));
    },
    
    displayAddedReminder: function (reminderData) {
        if (!reminderData) return; // Form was cancelled
        
        var reminderViewModel = this.createReminderViewModel(reminderData);
        this.reminders.push(reminderViewModel);
        
        if (!this.selectedReminder()) {
            this.selectedReminder(reminderViewModel);
        }
    },
    
    showReminderDetails: function (reminder) {
        this.selectedReminder(reminder);
    },
    
    createReminderViewModel: function (data) {
        var reminder = Reminder.create(data, this.http);
        reminder.isFulfilled.subscribe(function () {
            this.flashMessage.show("Reminder fulfilled");
            this.reminders.remove(reminder);
            this.selectedReminder(this.reminders()[0]);
        }, this);
        return reminder;
    }
    
});