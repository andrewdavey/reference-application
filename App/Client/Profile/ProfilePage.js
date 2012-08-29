/// <reference path="../Shared/Base.js"/>
/// <reference path="ProfileForm.js"/>

var ProfilePage = Base.inherit({

    templateId: "Client/Profile/ProfilePage.htm",
    
    init: function (viewData, app) {
        this.app = app;
        this.form = ProfileForm.create(viewData);
        this.form.onSaved.subscribe(this.goToDashboard.bind(this));
    },
    
    goToDashboard: function () {
        this.app.navigate("/");
    }
    
});