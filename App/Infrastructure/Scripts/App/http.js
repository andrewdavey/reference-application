var http = function(action, data) {
    return $.ajax({
        type: action.method,
        url: action.url,
        data: data,
        // Assign context for chained callbacks.
        context: this
    });
};