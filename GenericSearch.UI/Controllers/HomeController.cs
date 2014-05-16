using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.UI.Models;

namespace GenericSearch.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository repository;

        public HomeController()
        {
            this.repository = new Repository();
        }

        public ActionResult Index()
        {
            var data = this.repository
                .GetQuery()
                .ToArray();

            var model = new HomeModel()
            {
                Data = data,
                SearchCriteria = typeof(GenericSearch.Data.SomeClass).GetDefaultSearchCriterias(),

                DataNested = data.Select(d => Convert(d)),
                SearchCriteriaNested = typeof(GenericSearch.Data.SomeClass)
                    .GetDefaultSearchCriterias()
                    .AddCustomSearchCriteria<GenericSearch.Data.SomeClass>(s => s.Nested.TextNested)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ICollection<AbstractSearch> searchCriteria, ICollection<AbstractSearch> searchCriteriaNested, string custom)
        {
            var data = this.repository
                .GetQuery()
                .ApplySearchCriterias(searchCriteria)
                .ToArray();

            var dataNested = this.repository
                .GetQuery()
                .ApplySearchCriterias(searchCriteriaNested)
                .ToArray();

            var model = new HomeModel()
            {
                Data = data,
                SearchCriteria = searchCriteria,

                DataNested = dataNested.Select(d => Convert(d)),
                SearchCriteriaNested = searchCriteriaNested,

                SelectedTab = string.IsNullOrEmpty(custom) ? 0 : 1
            };

            return View(model);
        }

        private FlatSomeClass Convert(SomeClass input)
        {
            return new FlatSomeClass()
            {
                Date = input.Date,
                DateNullable = input.DateNullable,
                Integer = input.Integer,
                IntegerNullable = input.IntegerNullable,
                MyEnum = input.MyEnum,
                Text = input.Text,
                TextNested = input.Nested == null ? null : input.Nested.TextNested
            };
        }
    }
}
