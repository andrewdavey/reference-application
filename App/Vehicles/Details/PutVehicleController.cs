using System;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Http;
using MileageStats.Domain.Handlers;
using System.Net.Http;
using System.Threading.Tasks;

namespace App.Vehicles.Details
{
    public class PutVehicleController : ApiController
    {
        readonly UpdateVehicle updateVehicle;

        public PutVehicleController(UpdateVehicle updateVehicle)
        {
            this.updateVehicle = updateVehicle;
        }

        public async Task PutVehicle(int vehicleId)
        {
            var update = new VehicleUpdate
            {
                VehicleId = vehicleId
            };

            var parts = await Request.Content.ReadAsMultipartAsync();
            foreach (var part in parts)
            {
                var value = await part.ReadAsStringAsync();
                switch (part.Headers.ContentDisposition.Name.Trim('"'))
                {
                    case "name":
                        update.Name = value;
                        break;
                    case "year":
                        update.Year = int.Parse(value);
                        break;
                    case "make":
                        update.Make = value;
                        break;
                    case "model":
                        update.Model = value;
                        break;
                    case "photo":
                        update.Photo = new PhotoFile(part);
                        break;
                }
            }
            updateVehicle.Execute(1, update, update.Photo);
        }
    }

    class PhotoFile : HttpPostedFileBase
    {
        readonly Stream stream;
        readonly string contentType;

        public PhotoFile(HttpContent part)
        {
            contentType = part.Headers.ContentType.MediaType;
            stream = new MemoryStream();
            part.CopyToAsync(stream).Wait();
        }

        public override Stream InputStream
        {
            get
            {
                return stream;
            }
        }

        public override string ContentType
        {
            get { return contentType; }
        }
    }
}