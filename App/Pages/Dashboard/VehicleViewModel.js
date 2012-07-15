/// <reference path="~/Modules/Common/Object.js"/>

var VehicleViewModel = Object.inherit({
    
    init: function (data) {
        this.name = data.name;
        this.href = data.href;
    }
    
});