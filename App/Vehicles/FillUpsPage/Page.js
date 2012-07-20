/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="FillUp.js"/>

var Page = Object.inherit({
    
    templateId: "Vehicles/FillUpsPage/Page.htm",
    
    init: function (pageData) {
        this.fillUps = pageData.fillUps.map(FillUp.create);
        this.selectedFillUp = ko.observable();
        
        if (this.fillUps.length) {
            this.showFillUpDetails(this.fillUps[0]);
        }
    },
    
    showFillUpDetails: function (fillUp) {
        this.selectedFillUp(fillUp);
    }
    
});