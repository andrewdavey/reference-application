using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Server.ReferenceData
{
    public class GetYearsController : ApiController
    {
        readonly GetYearsMakesAndModels getYearsMakesAndModels;

        public GetYearsController(GetYearsMakesAndModels getYearsMakesAndModels)
        {
            this.getYearsMakesAndModels = getYearsMakesAndModels;
        }

        public IEnumerable<object> GetYears()
        {
            var years = getYearsMakesAndModels.Execute().Item1;

            return years.Select(year => new
            {
                year,
                makes = Url.Get<GetMakesController>(new { year })
            });
        }
    }
}