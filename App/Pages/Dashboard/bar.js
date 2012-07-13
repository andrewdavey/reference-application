/// <reference path="~/Infrastructure/Scripts/Object.js"/>
/// <reference path="foo.js"/>

var Bar = Object.inherit({
    initialize: function() {
        this.foo = foo;
    }
});