/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="AddReminderForm.js"/>

var RemindersPage = Object.inherit({
    init: function (viewData) {
        this.reminders = ko.observableArray(viewData.reminders);
        this.add = viewData.add;
    },
    
    templateId: "Vehicles/Reminders/RemindersPage.htm",
    
    showAddReminderForm: function () {
        var form = AddReminderForm.create({ add: this.add });
        form.show()
            .done(this.displayAddedReminder.bind(this));
    },
    
    displayAddedReminder: function (reminder) {
        if (!reminder) return; // Form was cancelled

        this.reminders.push(reminder);
    }
    
});