/// <reference path="~/Infrastructure/Scripts/Vendor/jquery.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/bootstrap/js/bootstrap.js"/>

var popups = (function() {

    var renderTemplate = function(templateName, viewModel) {
        // Use knockout's template binding to render the template into a hidden temporary div.
        var temporaryDiv = $("<div/>").hide().appendTo("body");
        ko.applyBindingsToNode(
            temporaryDiv[0],
            { template: { name: templateName, data: viewModel } },
            { }
        );

        // Get the modal div and move it to be a direct child of body.
        var modal = temporaryDiv.children().eq(0);
        modal.hide().appendTo("body");

        temporaryDiv.remove();
        return modal;
    };

    var hideCurrent = function() {
        if (popups.current) {
            popups.current.remove();
            delete popups.current;
        }
    };

    var popups = {
        renderTemplate: renderTemplate,
        hideCurrent: hideCurrent
    };
    
    popups.modal = function (viewModel) {
        popups.hideCurrent();

        var modalDiv;

        var remove = function () {
            modalDiv.modal("hide"); // this will then trigger the actual removal.
        };

        viewModel.__removeModal__ = remove;
        
        modalDiv = popups.renderTemplate(viewModel.templateId, viewModel);
        modalDiv.on("hidden", function () {
            modalDiv.remove();
        });

        // Show the modal
        modalDiv.modal();

        popups.current = {
            remove: remove
        };
    };

    popups.closeModal = function(viewModel) {
        viewModel.__removeModal__();
    };

    return popups;
}());