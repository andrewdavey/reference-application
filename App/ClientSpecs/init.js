/// <reference path="~/Client/Vendor/jquery.js"/>

$(function () {

    // Bit of a hack to get the require.js module paths configuration
    // See: https://groups.google.com/forum/?fromgroups=#!topic/requirejs/Hf-qNmM0ceI
    // We use this to look for specs to run.
    // Specs are modules who's paths end with "Specs" e.g. "Client/Specs/DashboardSpecs"
    var paths = requirejs.s.contexts._.config.paths;
    var specModules = Object
        .keys(paths)
        .filter(function(key) { return key.match(/Specs$/); });

    require(specModules, function() {
        var jasmineEnv = jasmine.getEnv();
        var htmlReporter = new jasmine.HtmlReporter();
        jasmineEnv.updateInterval = 1000;
        jasmineEnv.addReporter(htmlReporter);
        jasmineEnv.specFilter = function (spec) {
            return htmlReporter.specFilter(spec);
        };
        jasmineEnv.execute();
    });

});