/// <reference path="~/Modules/Common/Object.js"/>
/// <reference path="foo.js"/>

var Bar = Object.inherit({
    initialize: function() {
        this.foo = foo;
    }
});