/// <reference path="jasmine/specs.js"/>
/// <reference path="jasmine/matchers.js"/>
/// <reference path="jasmine/jasmine.js"/>
/// <reference path="jasmine/mockHttp.js"/>

specs.define(["Vehicles/FillUpsPage"], function (module) {
    var AddFillUpForm = module.AddFillUpForm;
    
    describe("AddFillUpForm", function () {
        var form;

        beforeEach(function () {
            require(["Infrastructure/Scripts/Vendor/moment"], function (moment) {
                moment.fn.sod = function() { return moment(new Date(2012, 6, 22, 0, 0, 0)); };
            });
            form = AddFillUpForm.create({ method: "post", url: "/fillups" });
        });


        describe("properties", function() {
            it("has date input", function() {
                expect(form.date).toBeObservable();
            });
            it("defaults date to today", function() {
                expect(form.date()).toBe("22 July 2012");
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
                expect(form.pricePerUnit.validation.message()).toBe("Price per unit is required");
            });
            it("requires total units", function () {
                form.totalUnits("");
                expect(form.totalUnits.validation.message()).toBe("Total units is required");
            });
            it("requires transaction fee", function () {
                form.transactionFee("");
                expect(form.transactionFee.validation.message()).toBe("Transaction fee is required");
            });
        });


        describe("save", function () {
            var savedData;
            beforeEach(function() {
                mockHttp.reset();
                mockHttp.post("/fillups").calls(function(_, data) {
                    savedData = data;
                });
            });
            
            it("saves inputed data", function () {
                form.date("2012/07/22");
                form.odometer("1000");
                form.pricePerUnit("0.50");
                form.totalUnits("100");
                form.transactionFee("1");
                form.remarks("remarks");
                form.vendor("vendor");
                form.save();
                
                expect(savedData).toEqual({
                    Date: "2012-07-22", // ISO-8601 compatible date string
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
});