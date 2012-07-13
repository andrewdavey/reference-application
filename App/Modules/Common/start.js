/// <reference path="Application.js" />
/// <reference path="Navigation.js" />
/// <reference path="FlashMessage.js" />

var start = function () {
    var app = Application.create(document);
    app.navigation = Navigation.create(app);
    app.flashMessage = FlashMessage.create();
    app.start();
};