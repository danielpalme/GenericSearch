using System.Collections.Generic;
using System.Linq;
using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.UI.Controllers
{
    public class DefaultSearchController : Controller
    {
        private readonly Repository repository;

        public DefaultSearchController()
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