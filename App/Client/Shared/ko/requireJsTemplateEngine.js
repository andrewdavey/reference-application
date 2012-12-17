/// <reference path="~/Client/Vendor/require.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="../Base.js"/>

var requireJsTemplateEngine = (function () {

    var TemplateSource = Base.inherit({
        init: function (templateId, options) {
            this.templateId = templateId;
            this.loaded = false;
            this.inProgress = false;
            this.template = ko.observable("loading...");
            this.template.data = {}; // Meta-data about the template

            if (options.afterRender) {
                var originalCallback = options.afterRender;
                options.afterRender = function () {
                    if (this.loaded) {
                        originalCallback.apply(options, arguments);
                    }
                }.bind(this);
            }
        },
    
        // Read/write meta-data about the template
        data: function (key, value) {
            if (arguments.length === 1) {
                return this.template.data[key];
            } else {
                this.template.data[key] = value;
            }
        },
    
        //read/write the actual template text
        text: function (value) {
            if (!this.loaded) {
                this.getTemplate();
            }

            if (arguments.length === 0) {
                return this.template();
            } else {
                this.template(value);
            }
        },
    
        getTemplate: function() {
            if (this.inProgress) return;
            this.inProgress = true;

            var templateModulePath = this.templateId.replace(".htm", "-template");
            require(
                [templateModulePath],
                this.templateDownloaded.bind(this)
            );
        },
    
        templateDownloaded: function(template) {
            this.inProgress = false;
            this.loaded = true;
            this.template(template);
        }
    });

    var engine = new ko.nativeTemplateEngine();
    var templates = {};

    engine.makeTemplateSource = function (template, bindingContext, options) {
        if (typeof template == "string") {
            // Named template
            var element = document.getElementById(template);
            if (element) {
                // The template already exists in the DOM, so can use it immediately.
                return new ko.templateSources.domElement(element);
            } else {
                if (!templates[template]) {
                    templates[template] = TemplateSource.create(template, options);
                }
                return templates[template];
            }
        } else if ((template.nodeType == 1) || (template.nodeType == 8)) {
            // Anonymous template
            return new ko.templateSources.anonymousTemplate(template);
        }
    };

    engine.renderTemplate = function (template, bindingContext, options) {
        var templateSource = engine.makeTemplateSource(template, bindingContext, options);
        return engine.renderTemplateSource(templateSource, bindingContext, options);
    };

    return engine;
}());