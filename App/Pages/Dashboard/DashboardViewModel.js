/// <reference path="~/Modules/Common/Object.js"/>

var DashboardViewModel = Object.inherit({
    init: function(pageData) {
        this.statistics = pageData.statistics;
    },
    
    templateId: "Pages/Dashboard/dashboard.htm"
});