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
    public class PostVehiclesController : ApiController
    {
        readonly CreateVehicle createVehicle;

        public PostVehiclesController(CreateVehicle createVehicle)
        {
            this.createVehicle = createVehicle;
        }

        public async Task<HttpResponseMessage> PostVehicle()
        {
            var vehicleId = await CreateVehicle();

            var vehicleUrl = Url.Resource<GetVehicleController>(new {vehicleId});

            // TODO: Clean up this hacky code
            // The response will be sent to an iframe, which can't access HTTP headers.
            // Therefore, we duplicate the header data in the response body.
            var response = Request.CreateResponse(
                HttpStatusCode.Created,
                new { headers = new { Location = vehicleUrl } }
            );
            response.Headers.Location = new Uri(vehicleUrl, UriKind.Relative);
            return response;
        }

        async Task<int> CreateVehicle()
        {
            var streamProvider = new MultipartFormDataStreamProvider(Path.GetTempPath());
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            int vehicleId;
            using (var form = new NewVehicleForm(streamProvider))
            {
                vehicleId = createVehicle.Execute(1, form, form.Photo);
            }
            return vehicleId;
        }

        class NewVehicleForm : ICreateVehicleCommand, IDisposable
        {
            readonly string localFileName;
            readonly FileStream file;

            public NewVehicleForm(MultipartFormDataStreamProvider streamProvider)
            {
                Name = streamProvider.FormData["Name"];
                Year = int.Parse(streamProvider.FormData["Year"]);
                Make = streamProvider.FormData["MakeName"];
                Model = streamProvider.FormData["ModelName"];

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