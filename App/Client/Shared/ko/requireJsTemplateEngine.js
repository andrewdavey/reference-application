/// <reference path="~/Client/Vendor/require.js"/>
/// <reference path="~/Client/Vendor/knockout.js"/>
/// <reference path="../Base.js"/>

var TemplateSource = Base.inherit({
    init: function (templateId) {
        this.templateId = templateId;
        this.loaded = false;
        this.inProgress = false;
        this.template = ko.observable("loading...");
        this.template.data = {}; // Meta-data about the template
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
        this.template(template);
        this.inProgress = false;
        this.loaded = true;
    }
});

var requireJsTemplateEngine = new ko.nativeTemplateEngine();
requireJsTemplateEngine.templateSources = {};
requireJsTemplateEngine.makeTemplateSource = function (templateId) {
    if (templateId in requireJsTemplateEngine.templateSources) {
        return requireJsTemplateEngine.templateSources[templateId];
    }
    
    var element = document.getElementById(templateId);
    if (element) {
        return new ko.templateSources.domElement(element);
    } else {
        return TemplateSource.create(templateId);
    }
};