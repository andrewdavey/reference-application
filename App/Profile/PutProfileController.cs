using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Profile
{
    public class PutProfileController : ApiController
    {
        readonly UpdateUser updateUser;

        public PutProfileController(UpdateUser updateUser)
        {
            this.updateUser = updateUser;
        }

        public void PutProfile()
        {
            //updateUser.Execute(new User { });
        }
    }
}