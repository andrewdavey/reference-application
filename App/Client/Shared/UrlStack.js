// UrlStack contains a stack of URLs that form a parent-child hirerarchy.
// For example: [ "/application", "/vehicles/masterpage", "/vehicle/123/fillups" ]
//
// Each URLs is downloaded, and the response data is stored.
// The response data can contain a `parent` property, indicating the parent URL.
// GET "/vehicles/masterpage" returns { parent: "/application", ... }
// GET "/vehicle/123/fillups" returns { parent: "/vehicles/masterpage", ... }
// 
// It provides a `navigate` method that effectively pops the stack until a commmon
// parent URL is found.
// For example:
//     navigate("/vehicle/456/reminders");
// will pop "/vehicle/123/fillups" off the stack
// and update the stack to be:
//     [ "/application", "/vehicles/masterpage", "/vehicle/456/reminders" ]

var UrlStack = Object.inherit({

    init: function (download) {
        /// <param name="download">Function that downloads a URL and returns a deferred object representing the download.</param>

        this.urls = []; // the URL currently on the stack.
        this.downloaded = {}; // set of downloaded URLs { "/url1": true, "/url2": true }
        this.download = download;
    },

    navigate: function (newUrl) {
        // newUrls will contain the newly downloaded URLs,
        // ordered parent-first e.g. [ "/root", "/master", "/details" ]
        var newUrls = [];

        var startDownload = function (url) {
            newUrls.unshift(url);
            return this.download(url).pipe(downloadCompleted);
        }.bind(this);

        var downloadCompleted = function (downloadedData) {
            var response = {
                url: newUrls[0],
                body: downloadedData,
                childUrl: newUrls[1]
            };
            var process = this.processDownloadResponse(response);
            return process.pipe(function(response) {
                if (response.parentUrl) {
                    if (this.contains(response.parentUrl)) {
                        // No need to download any more of the parent chain.
                        // Return the parent's URL so we know to only unload the
                        // the URLs that came after the parent.
                        // The new URLs will be added after the parent.
                        return response.parentUrl;
                    } else {
                        // Recursively download the parent.
                        return startDownload(response.parentUrl);
                    }
                } else {
                    // No common parent URL was found, so return null.
                    // This will cause the entire stack to be popped
                    // and replaced with the new URLs.
                    return null;
                }
            });
        };

        var finishedAllDownloads = function (commonParentUrl) {
            this.popUrlsUntil(commonParentUrl);
            this.urls = this.urls.concat(newUrls);
        }.bind(this);

        return startDownload(newUrl).done(finishedAllDownloads);
    },

    processDownloadResponse: function (response) {
        // Implemented as a deferred operation so subclasses of UrlStack
        // can perform async processing here. 
        
        var process = $.Deferred();
        
        this.downloaded[response.url] = true;
        response.parentUrl = response.body.parent;

        process.resolveWith(this, [response]);
        return process;
    },
    
    contains: function (url) {
        return this.downloaded.hasOwnProperty(url);
    },

    popUrlsUntil: function (targetUrl) {
        // Look backwards through the URLs, until we find the targetUrl.
        // Each URL after the target is unloaded.
        // If targetUrl is null, then all URLs are unloaded.

        // Before:
        //   this.urls == [ a, b, c, d ]
        //   targetUrl == b
        // After:
        //  this.urls == [a, b]

        var i = this.urls.length;
        var url;
        while (--i >= 0) {
            url = this.urls[i];
            if (url === targetUrl) {
                break;
            }

            this.removeUrl(url);
            this.urls.pop();
        }
    },

    removeUrl: function (url) {
        delete this.downloaded[url];
        if (typeof this.onUrlPopped === "function") {
            this.onUrlPopped(url);
        }
    }
});