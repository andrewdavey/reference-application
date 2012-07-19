using System.Collections.Generic;
using System.Web.Http;
using MileageStats.Domain.Handlers;

namespace App.Vehicles.ReferenceData
{
    public class ModelsController : ApiController
    {
        readonly GetYearsMakesAndModels getYearsMakesAndModels;

        public ModelsController(GetYearsMakesAndModels getYearsMakesAndModels)
        {
            this.getYearsMakesAndModels = getYearsMakesAndModels;
        }

        public IEnumerable<string> GetModels(int year, string make)
        {
            var models = getYearsMakesAndModels.Execute(year, make).Item3;
            return models;
        }
    }
}