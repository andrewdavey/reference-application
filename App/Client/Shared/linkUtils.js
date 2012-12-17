// A link object has the form { <HTTP_Method> : "URL" }
// e.g. { get: "/foo" } or { put: "/bar" }
// This object provides functions to read this information

var linkUtils = {
    getMethod: function (linkData) {
        return Object.keys(linkData)[0];
    },

    getUrl: function (linkData) {
        return linkData[linkUtils.getMethod(linkData)];
    }
};