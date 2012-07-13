/// <reference path="Object.js" />

var Event = Object.inherit({

    init: function () {
        this.handlers = [];
    },

    subscribe: function (handler) {
        this.handlers.push(handler);
    },

    trigger: function () {
        var eventData = arguments;
        this.handlers.forEach(function (handler) {
            handler.apply(null, eventData);
        });
    }

});