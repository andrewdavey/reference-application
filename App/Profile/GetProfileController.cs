using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Profile
{
    public class GetProfileController : ApiController
    {
        readonly GetUserByClaimId getUser;
        readonly UpdateUser updateUser;

        public GetProfileController(GetUserByClaimId getUser, UpdateUser updateUser)
        {
            this.getUser = getUser;
            this.updateUser = updateUser;
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

        public void PutProfile()
        {
            //updateUser.Execute(new User { });
        }
    }
}