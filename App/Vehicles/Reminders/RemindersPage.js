/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/moment.js"/>
/// <reference path="AddReminderForm.js"/>

var Reminder = Object.inherit({
    init: function(data) {
        this.title = data.Title;
        this.remarks = data.Remarks;
        this.dueDate = data.DueDate
            ? moment(data.DueDate).format("LL")
            : "(not entered)";
        this.dueDistance = data.DueDistance
            ? (data.DueDistance.toString() + " miles")
            : "(not entered)";
        this.dueSummary = this.createDueSummary(data);
    },
    
    createDueSummary: function (data) {
        var dueDateSummary = data.DueDate && "on " + this.dueDate;
        var dueDistanceSummary = data.DueDistance && "at " + this.dueDistance + " miles";

        return dueDateSummary && dueDistanceSummary ?
                   "Due " + dueDateSummary + " or " + dueDistanceSummary :
               dueDateSummary ?
                   "Due " + dueDateSummary :
               dueDistanceSummary ?
                   "Due " + dueDistanceSummary :
               "";
    }
});

var RemindersPage = Object.inherit({
    init: function (viewData) {
        this.reminders = ko.observableArray(viewData.reminders.map(Reminder.create));
        this.selectedReminder = ko.observable(this.reminders()[0]);
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

        this.reminders.push(reminder.create(reminder));
    },
    
    showReminderDetails: function (reminder) {
        this.selectedReminder(reminder);
    }
    
});