/// <reference path="../Shared/Base.js"/>
/// <reference path="ProfileForm.js"/>

var ProfilePage = Base.inherit({

    templateId: "Client/Profile/ProfilePage.htm",
    
    init: function (viewData, http) {
        this.form = ProfileForm.create(viewData, http);
        this.form.onSaved.subscribe(this.goToDashboard.bind(this));
    }
    
});