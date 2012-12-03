/// <reference path="~/Client/Shared/Base.js"/>

describe("mixin getValue function", function() {
    var Example = Base.inherit({
        init: function (v) { this.value = v; }
    }).mixin({
        getValue: function () {
            return this.value;
        }
    });

    var t1 = Example.create(1);
    var t2 = Example.create(2);

    it("Adds getValue function to prototype", function() {
        expect(typeof Example.getValue).toBe("function");
        expect(t1.getValue()).toBe(1);
        expect(t2.getValue()).toBe(2);
    });
});