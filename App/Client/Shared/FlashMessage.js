/// <reference path="../Vendor/knockout.js" />
/// <reference path="Base.js" />

var FlashMessage = Base.inherit({

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

var flashMessage = FlashMessage.create();