var httpCommand = function (httpMethodObject, context) {

    // httpMethodObject is an object with a single property 
    // representing the HTTP method and URL for a command.
    // e.g. { POST: "/some/url" }
    
    var httpMethod, url;
    for (httpMethod in httpMethodObject) {
        if (httpMethodObject.hasOwnProperty(httpMethod)) {
            url = httpMethodObject[httpMethod];
            break;
        }
    }

    return function (data) {
        return $.ajax({
            type: httpMethod,
            url: url,
            data: data,
            context: context
        });
    };
};