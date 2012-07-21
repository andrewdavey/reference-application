/// <reference path="jasmine.js" />
/// <reference path="~/Infrastructure/Scripts/Vendor/jquery.js" />

var specs = {
    
    definitions: [],
    
    define: function (deps, fn) {
        specs.definitions.push({ deps: deps, fn: fn });
    },
    
    initialize: function () {
        var initializing = $.Deferred();
        var count = specs.definitions.length;
        
        specs.definitions.forEach(function(def) {
            require(def.deps, function () {
                def.fn.apply(this, arguments);

                var allInitialized = --count === 0;
                if (allInitialized) {
                    initializing.resolve();
                }
            });
        });

        return initializing;
    }
    
};