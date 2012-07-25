/// <reference path="ViewDataStack.js" />

// Specializes ViewDataStack by converting raw view data into view model objects.
var ViewModelStack = ViewDataStack.inherit({
    init: function (app, http) {
        ViewDataStack.init.call(this, http);
        this.app = app;
    },

    createItem: function (url, data) {
        var creating = $.Deferred();
        var stack = this;
        require([data.Script], function (module) {
            var viewModel = module.init(data.Data, stack.app);
            creating.resolveWith(stack, [{ url: url, data: viewModel }]);
        });
        return creating;
    }
});