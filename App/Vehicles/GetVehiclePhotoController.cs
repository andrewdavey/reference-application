using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class GetVehiclePhotoController : ApiController
    {
        readonly GetVehicleById getVehicleById;
        readonly GetVehiclePhoto getVehiclePhoto;

        public GetVehiclePhotoController(GetVehicleById getVehicleById, GetVehiclePhoto getVehiclePhoto)
        {
            this.getVehicleById = getVehicleById;
            this.getVehiclePhoto = getVehiclePhoto;
        }

        public HttpResponseMessage GetPhoto(int vehicleId)
        {
            var vehicle = getVehicleById.Execute(1, vehicleId);
            if (vehicle == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);                
            }

            var photo = getVehiclePhoto.Execute(vehicleId);
            if (photo == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage
            {
                Content = new ByteArrayContent(photo.Image)
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue(photo.ImageMimeType)
                    }
                }
            };
        }
    }
}