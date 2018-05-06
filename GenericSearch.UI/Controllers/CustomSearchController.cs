using System.Collections.Generic;
using System.Linq;
using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.UI.Controllers
{
    public class CustomSearchController : Controller
    {
        private readonly Repository repository;

        public CustomSearchController()
        {
            this.repository = new Repository();
        }

        public ActionResult Index()
        {
            var data = this.repository
                .GetQuery()
                .ToArray();

            var model = new SearchViewModel()
            {
                Data = data,
                SearchCriteria = typeof(GenericSearch.Data.SomeClass)
                    .GetDefaultSearchCriteria()
                    .AddCustomSearchCriterion<GenericSearch.Data.SomeClass>(s => s.Nested.TextNested)
                    .AddCustomSearchCriterion<GenericSearch.Data.SomeClass>(s => s.CollectionComplex.Select(c => c.TextNested))
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ICollection<AbstractSearch> searchCriteria)
        {
            var data = this.repository
                .GetQuery()
                .ApplySearchCriteria(searchCriteria)
                .ToArray();

            var model = new SearchViewModel()
            {
                Data = data,
                SearchCriteria = searchCriteria
            };

            return this.View(model);
        }
    }
}