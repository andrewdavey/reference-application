/// <reference path="jasmine.js" />
/// <reference path="jasmine-html.js" />
/// <reference path="specs.js" />

var init = function () {
    // Wait for async specifications to run...
    specs.initialize().then(function () {
        var jasmineEnv = jasmine.getEnv();
        var htmlReporter = new jasmine.TrivialReporter();
        jasmineEnv.updateInterval = 1000;
        jasmineEnv.addReporter(htmlReporter);
        jasmineEnv.specFilter = function(spec) {
            return htmlReporter.specFilter(spec);
        };
        jasmineEnv.execute();
    });
};