/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/matchers.js"/>
/// <reference path="jasmine/jasmine.js"/>

specs.define(["Vehicles/NewFillUpPage"], function (module) {
    var NewFillUpForm = module.NewFillUpForm;
    
    describe("NewFillUpForm", function() {
        var form;

        beforeEach(function() {
            form = NewFillUpForm.create();
        });

        describe("properties", function() {
            it("has date input", function() {
                expect(form.date).toBeObservable();
            });
            it("has odometer input", function() {
                expect(form.odometer).toBeObservable();
            });
            it("has price per unit input", function() {
                expect(form.pricePerUnit).toBeObservable();
            });
            it("has total units input", function() {
                expect(form.totalUnits).toBeObservable();
            });
            it("has vendor input", function() {
                expect(form.vendor).toBeObservable();
            });
            it("has transaction fee input", function() {
                expect(form.transactionFee).toBeObservable();
            });
            it("has remarks input", function() {
                expect(form.remarks).toBeObservable();
            });
            it("computes total cost", function() {
                form.pricePerUnit(".1");
                form.totalUnits("100");
                form.transactionFee("5");
                expect(form.totalCost()).toBe("15.00");
            });
            it("has empty total cost when pricePerUnit is missing", function () {
                form.pricePerUnit("");
                expect(form.totalCost()).toBe("");
            });
            it("has empty total cost when totalUnits is missing", function () {
                form.totalUnits("");
                expect(form.totalCost()).toBe("");
            });
            it("has empty total cost when transactionFee is missing", function () {
                form.transactionFee("");
                expect(form.totalCost()).toBe("");
            });
        });

        describe("validation", function () {
            it("requires date", function () {
                form.date("");
                form.save();
                expect(form.date.validation.message()).toBe("Date is required");
            });
            it("odometer is required", function () {
                form.odometer("");
                expect(form.odometer.validation.message()).toBe("Odometer is required");
            });
            it("odometer must be a whole number", function() {
                form.odometer("123fail");
                expect(form.odometer.validation.message()).toBe("Enter a whole number");
            });
            it("requires price per unit", function () {
                form.pricePerUnit("");
                form.save();
                expect(form.pricePerUnit.validation.message()).toBe("Price per unit is required");
            });
            it("requires total units", function () {
                form.totalUnits("");
                form.save();
                expect(form.totalUnits.validation.message()).toBe("Total units is required");
            });
            it("requires transaction fee", function () {
                form.transactionFee("");
                form.save();
                expect(form.transactionFee.validation.message()).toBe("Transaction fee is required");
            });
        });
    });
    
});