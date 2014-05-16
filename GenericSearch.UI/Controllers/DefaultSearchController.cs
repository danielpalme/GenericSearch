using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.UI.Models;

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
                    .GetDefaultSearchCriterias()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(ICollection<AbstractSearch> searchCriteria)
        {
            var data = this.repository
                .GetQuery()
                .ApplySearchCriterias(searchCriteria)
                .ToArray();

            var model = new SearchViewModel()
            {
                Data = data,
                SearchCriteria = searchCriteria
            };

            return View(model);
        }
    }
}