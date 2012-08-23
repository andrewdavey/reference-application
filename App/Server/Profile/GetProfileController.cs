using System.Web.Http;
using App.Infrastructure.Web;
using App.Server.ReferenceData;
using MileageStats.Domain.Handlers;

namespace App.Server.Profile
{
    public class GetProfileController : ApiController
    {
        readonly GetUserByClaimId getUser;

        public GetProfileController(GetUserByClaimId getUser)
        {
            this.getUser = getUser;
        }

        public object GetProfile()
        {
            return new
            {
                name = "",
                countries = Url.Get<GetCountriesController>(),
                save = Url.Put<GetProfileController>()
            };
        }
    }
}