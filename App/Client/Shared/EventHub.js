var EventHub = Object.inherit({
    init: function () {
        this.handlers = { };
    },
    
    subscribe: function (eventName, handlerFunction) {
        if (this.handlers.hasOwnProperty(eventName)) {
            this.handlers[eventName].push(handlerFunction);
        } else {
            this.handlers[eventName] = [handlerFunction];
        }
    },
    
    publish: function (eventName, eventData) {
        var handlers = this.handlers[eventName] || [];
        handlers.forEach(function(handler) {
            handler(eventData);
        });
    }
});

var eventHub = EventHub.create();