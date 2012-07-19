using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Vehicles.ReferenceData
{
    public class YearsController : ApiController
    {
        readonly GetYearsMakesAndModels getYearsMakesAndModels;

        public YearsController(GetYearsMakesAndModels getYearsMakesAndModels)
        {
            this.getYearsMakesAndModels = getYearsMakesAndModels;
        }

        public IEnumerable<object> GetYears()
        {
            var years = getYearsMakesAndModels.Execute().Item1;

            return years.Select(year => new
            {
                year,
                makes = new { get = Url.Resource<MakesController>(new { year }) }
            });
        }
    }
}