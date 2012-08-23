using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using App.Infrastructure;
using App.Infrastructure.Web;
using MileageStats.Domain.Contracts;
using MileageStats.Domain.Handlers;

namespace App.Server.Vehicle
{
    public class PutVehicleController : ApiController
    {
        readonly UpdateVehicle updateVehicle;

        public PutVehicleController(UpdateVehicle updateVehicle)
        {
            this.updateVehicle = updateVehicle;
        }

        public async Task<HttpResponseMessage> PutVehicle(int vehicleId)
        {
            var streamProvider = new MultipartFormDataStreamProvider(Path.GetTempPath());
            await Request.Content.ReadAsMultipartAsync(streamProvider);
            
            using (var update = new VehicleUpdate(vehicleId, streamProvider))
            {
                updateVehicle.Execute(1, update, update.Photo);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        class VehicleUpdate : ICreateVehicleCommand, IDisposable
        {
            readonly FileStream file;
            readonly string localFileName;

            public VehicleUpdate(int vehicleId, MultipartFormDataStreamProvider streamProvider)
            {
                VehicleId = vehicleId;
                Name = streamProvider.FormData["Name"];
                Year = int.Parse(streamProvider.FormData["Year"]);
                Make = streamProvider.FormData["Make"];
                Model = streamProvider.FormData["Model"];

                localFileName = streamProvider.FileData[0].LocalFileName;
                file = File.OpenRead(localFileName);
                var mediaType = streamProvider.FileData[0].Headers.ContentType.MediaType;
                Photo = new FileWrapper(file, mediaType);
            }

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

            public void Dispose()
            {
                file.Dispose();
                File.Delete(localFileName);
            }
        }
    }
}