using System.Web.Http;
using App.Infrastructure;
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
            var user = getUser.Execute("http://not/a/real/openid/url");
            return new Page("Profile/init")
            {
                Data = new
                {
                    name = user.DisplayName,
                    country = user.Country,
                    hasRegistered = user.HasRegistered,
                    countries = Url.Get<GetCountriesController>(),
                    save = Url.Put<GetProfileController>()
                }
            };
        }
    }
}