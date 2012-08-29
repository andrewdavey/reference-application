using System.Web.Http;
using MileageStats.Domain.Handlers;
using MileageStats.Domain.Models;

namespace App.Server.Profile
{
    public class PutProfileController : ApiController
    {
        readonly UpdateUser updateUser;

        public PutProfileController(UpdateUser updateUser)
        {
            this.updateUser = updateUser;
        }

        public void PutProfile(ProfileData data)
        {
            updateUser.Execute(new User
            {
                Id = 1,
                AuthorizationId = "http://not/a/real/openid/url",
                DisplayName = data.Name,
                Country = data.Country,
                HasRegistered = true
            });
        }

        public class ProfileData
        {
            public string Name { get; set; }
            public string Country { get; set; }
        }
    }
}