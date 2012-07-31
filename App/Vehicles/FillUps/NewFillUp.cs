using System;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Models;

namespace App.Vehicles.FillUps
{
    public class NewFillUp : ICreateFillupEntryCommand
    {
        public int FillupEntryId { get; set; }
        public int VehicleId { get; set; }
        public DateTime? Date { get; set; }
        public int Odometer { get; set; }
        public double? PricePerUnit { get; set; }
        public double? TotalUnits { get; set; }
        public FillupUnits UnitOfMeasure { get; set; }
        public string Vendor { get; set; }
        public string Location { get; set; }
        public double? TransactionFee { get; set; }
        public string Remarks { get; set; }
        public int? Distance { get; set; }
    }
}