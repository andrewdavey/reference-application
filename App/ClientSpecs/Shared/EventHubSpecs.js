/// <reference path="~/Client/Shared/EventHubFactory.js"/>

describe("eventHub", function() {
    var eventHub;
    
    describe("subscribe then publish", function () {
        var handlerCalled = false;
            
        beforeEach(function () {
            eventHub = EventHubFactory.create();
            eventHub.subscribe("test-event", function() { handlerCalled = true; });
            eventHub.publish("test-event");
        });
            
        it("calls handler", function() {
            expect(handlerCalled).toBeTruthy();
        });
    });

    describe("given no subscribers, publish an event", function() {
        beforeEach(function () {
            eventHub = EventHubFactory.create();
            eventHub.publish("test-event");
        });
            
        it("doesn't throw an exception", function() {});
    });

    describe("given two subscribers, publish calls both", function() {
        var handler1Called = false;
        var handler2Called = false;

        beforeEach(function () {
            eventHub = EventHubFactory.create();
            eventHub.subscribe("test-event", function () { handler1Called = true; });
            eventHub.subscribe("test-event", function () { handler2Called = true; });
            eventHub.publish("test-event");
        });

        it("calls handler1", function () {
            expect(handler1Called).toBeTruthy();
        });
        it("calls handler2", function () {
            expect(handler2Called).toBeTruthy();
        });
    });

    describe("publish with event data", function () {
        var sentData = {};
        var receivedData;
            
        beforeEach(function () {
            eventHub = EventHubFactory.create();
            eventHub.subscribe("test-event", function (data) { receivedData = data; });
            eventHub.publish("test-event", sentData);
        });

        it("it passes sent data to handler function", function () {
            expect(receivedData).toBe(sentData);
        });
    });
});