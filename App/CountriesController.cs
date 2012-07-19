using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App
{
    public class CountriesController : ApiController
    {
        readonly GetCountries getCountries;

        public CountriesController(GetCountries getCountries)
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