/// <reference path="../../Vendor/knockout.js"/>
/// <reference path="../../Vendor/moment.js"/>
/// <reference path="../../Vendor/bootstrap/js/datepicker.js"/>

ko.bindingHandlers["datepicker"] = {
    init: function(element, valueAccessor) {
        var picker = $(element).datepicker({
            autoclose: true,
            format: moment.longDateFormat.L.toLowerCase()
        });

        picker.on("changeDate", function (event) {
            valueAccessor()(event.date);
        });
    },
    
    update: function (element, valueAccessor) {
        $(element).datepicker("setDate", valueAccessor());
    }
};