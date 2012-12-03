/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="../Shared/Base.js"/>

var AppFrame = Base.inherit({
    
    init: function (viewData, flashMessage) {
        // Navigation links
        this.links = viewData.links;
        
        // Flash message used to display a short notification of some action.
        // e.g. "Profile saved"
        this.flashMessage = flashMessage;

        // The view model of whatever the AppFrame is containing.
        this.content = ko.observable();

        this.ensureLinksHaveRels();
    },
    
    templateId: "Client/AppFrame/AppFrame.htm",
    
    ensureLinksHaveRels: function () {
        // Binding `rel` to <a rel="..."> in the UI will fail if `rel` is undefined.
        this.links.forEach(function (link) {
            link.rel = link.rel || null;
        });
    }
    
});