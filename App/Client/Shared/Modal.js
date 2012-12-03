/// <reference path="~/Client/Vendor/jquery.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="~/Client/Vendor/bootstrap/js/bootstrap.js"/>
/// <reference path="Base.js"/>

// Creating a Modal displays a modal dialog UI.
// The contents are rendered from a template, which is data bound to a view model.
var Modal = Base.inherit({
    
    init: function (viewModel, templateId) {
        this.viewModel = viewModel;
        this.templateId = templateId || viewModel.templateId;
        this.element = this.createModalElement();
        
        // Create a Deferred object that is resolved when the modal is closed.
        // This allows calling code to chain callbacks to be run after the modal is closed.
        this.showing = $.Deferred();

        this.show();
    },
    
    closeWithResult: function (result) {
        this.hideModalElement();
        this.showing.resolve(result);
    },
    
    close: function () {
        this.hideModalElement();
        this.showing.resolve();
    },

    
    show: function () {
        var element = this.element;
        
        // Use the Twitter Bootstrap modal plugin to display the UI.
        element
            .modal()
            .on("hidden", function () {
                element.remove();
            });
    },
    
    createModalElement: function () {
        var temporaryDiv;
        try {
            temporaryDiv = this.createHiddenDiv();
            this.applyTemplateBinding(temporaryDiv);
            var modalElement = this.firstChildElement(temporaryDiv);
            modalElement
                .hide()
                .appendTo("body");

            return modalElement;
            
        } finally {
            if (temporaryDiv) temporaryDiv.remove();
        }
    },
    
    createHiddenDiv: function () {
        return $("<div/>").hide().appendTo("body");
    },
    
    applyTemplateBinding: function (targetElement) {
        ko.applyBindingsToNode(
            targetElement[0],
            { template: { name: this.templateId, data: this.viewModel } },
            {}
        );
    },
    
    firstChildElement: function (parent) {
        return parent.children().eq(0);
    },
    
    hideModalElement: function () {
        // Hiding the modal will trigger removal, due to the "hidden" handler defined in `this.show()`.
        this.element.modal("hide");
    }
    
});