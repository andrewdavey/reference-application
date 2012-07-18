using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using App.Infrastructure.Web;
using MileageStats.Domain.Handlers;

namespace App.Modules.ReferenceData
{
    public class MakesController : ApiController
    {
        readonly GetYearsMakesAndModels getYearsMakesAndModels;

        public MakesController(GetYearsMakesAndModels getYearsMakesAndModels)
        {
            this.getYearsMakesAndModels = getYearsMakesAndModels;
        }

        public IEnumerable<object> GetMakes(int year)
        {
            var makes = getYearsMakesAndModels.Execute(year).Item2;

            return makes.Select(make => new
            {
                make,
                models = new {get = Url.Resource<ModelsController>(new {year, make})}
            });
        }
    }
}