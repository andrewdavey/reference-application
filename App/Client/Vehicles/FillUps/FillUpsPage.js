/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Shared/Object.js"/>
/// <reference path="FillUp.js"/>
/// <reference path="AddFillUpForm.js" />

var FillUpsPage = Object.inherit({
    
    templateId: "Client/Vehicles/FillUps/FillUpsPage.htm",
    
    init: function (viewData, flashMessage) {
        this.addCommand = viewData.add;
        this.flashMessage = flashMessage;
        this.fillUps = ko.observableArray(viewData.fillUps.map(FillUp.create));
        this.selectedFillUp = ko.observable();
        
        if (this.fillUps().length) {
            this.showFillUpDetails(this.fillUps()[0]);
        }
    },
    
    showFillUpDetails: function (fillUp) {
        this.selectedFillUp(fillUp);
    },
    
    showAddFillUpForm: function () {
        var form = AddFillUpForm.create(this.addCommand);
        form.show()
            .done(this.insertNewFillUpAtTop.bind(this));
    },
    
    insertNewFillUpAtTop: function (fillUpData) {
        if (!fillUpData) return;
        
        var fillUp = FillUp.create(fillUpData);
        this.fillUps.splice(0, 0, fillUp);
        this.selectedFillUp(fillUp);
        this.flashMessage.show("Fill up added");
    }
    
});