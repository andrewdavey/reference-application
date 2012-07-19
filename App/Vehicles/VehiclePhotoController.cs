using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class VehiclePhotoController : ApiController
    {
        readonly GetVehiclePhoto getVehiclePhoto;

        public VehiclePhotoController(GetVehiclePhoto getVehiclePhoto)
        {
            this.getVehiclePhoto = getVehiclePhoto;
        }

        public HttpResponseMessage GetPhoto(int id)
        {
            var photo = getVehiclePhoto.Execute(id);
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