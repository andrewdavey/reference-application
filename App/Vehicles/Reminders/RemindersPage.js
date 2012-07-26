/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/moment.js"/>
/// <reference path="Reminder.js"/>
/// <reference path="AddReminderForm.js"/>

var RemindersPage = Object.inherit({
    init: function (viewData, flashMessage) {
        this.flashMessage = flashMessage;
        this.reminders = ko.observableArray(viewData.reminders.map(this.createReminderViewModel, this));
        this.selectedReminder = ko.observable(this.reminders()[0]);
        this.add = viewData.add;
    },
    
    templateId: "Vehicles/Reminders/RemindersPage.htm",
    
    showAddReminderForm: function () {
        var form = AddReminderForm.create({ add: this.add });
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
        var reminder = Reminder.create(data);
        reminder.isFulfilled.subscribe(function () {
            this.flashMessage.show("Reminder fulfilled");
            this.reminders.remove(reminder);
            this.selectedReminder(this.reminders()[0]);
        }, this);
        return reminder;
    }
    
});