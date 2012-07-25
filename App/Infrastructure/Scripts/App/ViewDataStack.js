/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>
/// <reference path="~/Infrastructure/Scripts/Vendor/jquery.js"/>

var ViewDataStack = Object.inherit({
    init: function (http) {
        this.http = http;
        this.top = null;
        this.viewDataByUrl = {};
    },

    download: function (url) {
        return this.http({ method: "get", url: url });
    },

    load: function (url, isNewTop) {
        return this
            .download(url)
            .pipe(function (response) {
                return this.createItem(url, response).pipe(function (item) {
                    if (isNewTop) {
                        this.current = this.newTop = item;
                    } else {
                        this.current.master = item;
                        this.current = item;
                    }
                    if (response.Master) {
                        var existingMaster = this.viewDataByUrl[response.Master];
                        if (existingMaster) {
                            item.master = existingMaster;
                            return this.newTop;
                        } else {
                            return this.load(response.Master);
                        }
                    } else {
                        return this.newTop;
                    }
                });
            });
    },
    
    createItem: function (url, data) {
        return $.Deferred().resolveWith(this, [{ url: url, data: data }]);
    },

    navigate: function (url) {
        return this.load(url, true).done(function (newTop) {
            this.top = newTop;
            this.rebuildViewDataByUrl();
        });
    },

    rebuildViewDataByUrl: function () {
        var current = this.top;
        this.viewDataByUrl = {};
        while (current) {
            this.viewDataByUrl[current.url] = current;
            current = current.master;
        }
    },

    toArray: function () {
        var array = [];
        var current = this.top;
        while (current) {
            array.push(current.data);
            current = current.master;
        }
        return array;
    }
});
