using MileageStats.Domain.Contracts;

namespace App.Modules.Vehicles
{
    public class NewVehicleForm : ICreateVehicleCommand
    {
        public int VehicleId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int? Year { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
    }
}