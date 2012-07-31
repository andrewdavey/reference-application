using System.Web;
using MileageStats.Domain.Contracts;

namespace App.Vehicles.Details
{
    public class VehicleUpdate : ICreateVehicleCommand
    {
        public int VehicleId { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public int? Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public HttpPostedFileBase Photo { get; set; }

        string ICreateVehicleCommand.MakeName
        {
            get { return Make; }
            set { Make = value; }
        }

        string ICreateVehicleCommand.ModelName
        {
            get { return Model; }
            set { Model = value; }
        }
    }
}