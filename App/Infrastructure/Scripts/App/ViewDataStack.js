/// <reference path="~/Infrastructure/Scripts/App/Object.js"/>

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
                var item = { url: url, data: response };
                if (isNewTop) {
                    this.current = this.newTop = item;
                } else {
                    this.current.master = item;
                    this.current = item;
                }
                if (response.master) {
                    var existingMaster = this.viewDataByUrl[response.master];
                    if (existingMaster) {
                        item.master = existingMaster;
                        return this.newTop;
                    } else {
                        return this.load(response.master);
                    }
                } else {
                    return this.newTop;
                }
            });
    },

    navigate: function (url) {
        this.load(url, true).done(function (newTop) {
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
