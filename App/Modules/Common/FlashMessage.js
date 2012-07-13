/// <reference path="~/Infrastructure/Scripts/knockout.js" />
/// <reference path="Object.js" />

var FlashMessage = Object.inherit({

    init: function () {
        this.message = ko.observable();
        this.visible = ko.observable(false);
    },

    show: function (message) {
        this.message(message);
        this.visible(true);
        setTimeout(this.hide.bind(this), 2000);
    },

    hide: function () {
        this.visible(false);
    }

});