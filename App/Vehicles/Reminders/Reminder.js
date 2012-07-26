/// <reference path="~/Infrastructure/Scripts/App/http.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/moment.js"/>
/// <reference path="AddReminderForm.js"/>

var Reminder = Object.inherit({
    init: function (data) {
        this.http = http;
        this.title = data.Title;
        this.remarks = data.Remarks;
        this.isOverdue = data.IsOverdue;
        this.isFulfilled = ko.observable(data.IsFulfilled);
        this.dueDate = data.DueDate
            ? moment(data.DueDate).format("LL")
            : "(not entered)";
        this.dueDistance = data.DueDistance
            ? (data.DueDistance.toString() + " miles")
            : "(not entered)";
        this.dueSummary = this.createDueSummary(data);
        this.updateCommand = data.Update;
    },
    
    createDueSummary: function (data) {
        var dueDateSummary = data.DueDate && "on " + this.dueDate;
        var dueDistanceSummary = data.DueDistance && "at " + this.dueDistance;

        return dueDateSummary && dueDistanceSummary ?
                   "Due " + dueDateSummary + " or " + dueDistanceSummary :
               dueDateSummary ?
                   "Due " + dueDateSummary :
               dueDistanceSummary ?
                   "Due " + dueDistanceSummary :
               "";
    },
    
    fulfill: function () {
        this.http(this.updateCommand, { isFulfilled: true })
            .done(function() {
                this.isFulfilled(true);
            });
    }
});