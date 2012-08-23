using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Server.ReferenceData
{
    public class GetCountriesController : ApiController
    {
        readonly GetCountries getCountries;

        public GetCountriesController(GetCountries getCountries)
        {
            this.getCountries = getCountries;
        }

        public object GetCountries()
        {
            return new
            {
                countries = getCountries.Execute()
            };
        }
    }
}