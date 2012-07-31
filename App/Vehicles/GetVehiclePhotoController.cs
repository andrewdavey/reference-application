using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles
{
    public class GetVehiclePhotoController : ApiController
    {
        readonly GetVehiclePhoto getVehiclePhoto;

        public GetVehiclePhotoController(GetVehiclePhoto getVehiclePhoto)
        {
            this.getVehiclePhoto = getVehiclePhoto;
        }

        public HttpResponseMessage GetPhoto(int photoId)
        {
            var photo = getVehiclePhoto.Execute(photoId);
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