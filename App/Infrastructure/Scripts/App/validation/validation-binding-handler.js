/// <reference path="~/Infrastructure/Scripts/Vendor/knockout.js"/>

// A KnockoutJS binding handler called `validation`.
// Usage:
// <div data-bind="validation: observableProperty">
//   <div class="controls">
//     <input type="text" data-bind="value: observableProperty"/>
//     <!-- An error message span will be generated here -->
//   </div>
// </div>

(function() {

    var messageSpanClass = "validation-error-message";

    var createErrorMessageSpan = function () {
        var messageSpan = document.createElement("span");
        messageSpan.setAttribute("class", "help-inline " + messageSpanClass);
        return messageSpan;
    };

    var addErrorMessageSpan = function(element) {
        var messageSpan = createErrorMessageSpan();
        element.appendChild(messageSpan);
    };

    var bindCssErrorClass = function (element, validation) {
        // Approximates having the following HTML in the DOM:
        // <div data-bind="css: { error: validation.isInvalid }"></div>
        ko.bindingHandlers.css.update(element, function () {
            return { error: validation.isInvalid };
        });
    };

    var bindTextToValidationMessage = function(element, validation) {
        var span = element.querySelector("." + messageSpanClass);
        // Approximates having the following HTML in the DOM:
        // <span data-bind="text: validation.message"></span>
        ko.bindingHandlers.text.update(span, validation.message);
    };
    
    ko.bindingHandlers.validation = {
        init: function (element, valueAccessor) {
            var validation = valueAccessor().validation;
            if (!validation) return;
            
            var controlsDiv = element.querySelector(".controls");
            addErrorMessageSpan(controlsDiv);
        },

        update: function(element, valueAccessor) {
            var validation = valueAccessor().validation;
            if (!validation) return;
            
            bindCssErrorClass(element, validation);
            bindTextToValidationMessage(element, validation);
        }
    };

}());