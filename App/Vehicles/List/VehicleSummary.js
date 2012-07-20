/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>

var VehicleSummary = Object.inherit({
    init: function (data) {
        for (var property in data) {
            if (data.hasOwnProperty(property)) {
                this[property] = data[property];
            }
        }
        this.year = this.year || "";
        this.make = this.make || "";
        this.model = this.model || "";
    }
})