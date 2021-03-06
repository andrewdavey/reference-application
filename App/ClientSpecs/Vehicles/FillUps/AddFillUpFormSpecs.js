﻿/// <reference path="~/Client/Vehicles/FillUps/AddFillUpForm.js"/>
/// <reference path="~/Client/Shared/Modal.js"/>
/// <reference path="~/Client/Vendor/moment.js"/>
/// <reference path="~/ClientSpecs/Shared/MockHttp.js"/>

// Stub out the Modal constructor, since the real object creates UI.
Modal.create = function() {
    return {
        result: $.Deferred(),
        close: function () { },
        closeWithResult: function () {}
    };
};

describe("AddFillUpForm", function () {
    var form, mockHttp;

    beforeEach(function () {
        moment.fn.sod = function () { return moment(new Date(2012, 6, 22, 0, 0, 0)); };
        mockHttp = MockHttp.create();
        form = AddFillUpForm.create({ post: "/fillups" }, mockHttp.http);
        form.show();
    });

    describe("properties", function() {
        it("has date input", function() {
            expect(form.date).toBeObservable();
        });
        it("defaults date to today", function() {
            expect(form.date()).toEqual(moment().sod().toDate());
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
            form.pricePerUnit(.1);
            form.totalUnits(100);
            form.transactionFee(5);
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
            expect(form.date.validation.message()).toBe("Date is required");
        });
        it("odometer is required", function () {
            form.odometer(null);
            expect(form.odometer.validation.message()).toBe("Odometer is required");
        });
        it("odometer must be greater than 0", function() {
            form.odometer(-1);
            expect(form.odometer.validation.message()).toBe("Must be greater than 0");
        });
        it("requires price per unit", function () {
            form.pricePerUnit(null);
            expect(form.pricePerUnit.validation.message()).toBe("Price per unit is required");
        });
        it("requires total units", function () {
            form.totalUnits(null);
            expect(form.totalUnits.validation.message()).toBe("Total units is required");
        });
        it("requires transaction fee", function () {
            form.transactionFee(null);
            expect(form.transactionFee.validation.message()).toBe("Transaction fee is required");
        });
    });


    describe("save", function () {
        var savedData;
        beforeEach(function() {
            mockHttp.post("/fillups").calls(function(_, data) {
                savedData = data;
            });
        });
            
        it("saves inputed data", function () {
            form.date(new Date(2012, 6, 22, 0, 0, 0));
            form.odometer(1000);
            form.pricePerUnit(0.5);
            form.totalUnits(100);
            form.transactionFee(1);
            form.remarks("remarks");
            form.vendor("vendor");
            form.save();
                
            expect(savedData).toEqual({
                Date: new Date(2012, 6, 22, 0, 0, 0),
                Odometer: 1000,
                PricePerUnit: 0.5,
                TotalUnits: 100,
                TransactionFee: 1,
                Remarks: "remarks",
                Vendor: "vendor"
            });
        });
    });
        
});