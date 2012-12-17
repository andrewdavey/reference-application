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
        // Create a Deferred object that is resolved when the modal is closed.
        // This allows calling code to chain callbacks to be run after the modal is closed.
        this.result = $.Deferred();
        this.createAndShowModal();
    },

    createAndShowModal: function () {
        this.createModalElement()
            .pipe($)
            .done(this.show.bind(this));
    },
    
    show: function (element) {
        this.element = element;
        // Use the Twitter Bootstrap modal plugin to display the UI.
        this.element
            .modal()
            .on("hidden", function () {
                this.element.remove();
                if (!this.result.isResolved && !this.result.isRejected) {
                    this.result.resolve();
                }
            }.bind(this));
    },

    createModalElement: function () {
        var temporaryDiv = this.addHiddenDivToBody();
        var deferredElement = $.Deferred();
        ko.renderTemplate(
            this.templateId,
            this.viewModel,
            // We need to know when the template has been rendered,
            // so we can get the resulting DOM element.
            // The resolve function receives the element.
            {
                afterRender: function (nodes) {
                    // Ignore any #text nodes before and after the modal element.
                    // We only want the modal's <div> element.
                    var elements = [].filter.call(nodes, function (node) {
                        return node.nodeType === 1; // Element
                    });
                    deferredElement.resolve(elements[0]);
                }
            },
            // The temporary div will get replaced by the rendered template output.
            temporaryDiv,
            "replaceNode"
        );
        // Return the deferred DOM element so callers can wait until it's ready for use.
        return deferredElement;
    },

    addHiddenDivToBody: function () {
        var div = document.createElement("div");
        div.style.display = "none";
        document.body.appendChild(div);
        return div;
    },

    closeWithResult: function (result) {
        this.hideModalElement();
        this.result.resolve(result);
    },

    close: function () {
        this.hideModalElement();
        this.result.resolve();
    },

    hideModalElement: function () {
        // Hiding the modal will trigger removal, due to the "hidden" handler defined in `this.show()`.
        this.element.modal("hide");
    }
});