using System.Web.Mvc;
using GenericSearch.Data;
using GenericSearch.Grammar;
using GenericSearch.UI.Models;

namespace GenericSearch.UI.Controllers
{
    public class GrammarSearchController : Controller
    {
        private readonly Repository repository;

        public GrammarSearchController()
        {
            this.repository = new Repository();
        }

        public ActionResult Index(GrammarSearchViewModel input)
        {
            var model = new GrammarSearchViewModel();

            try
            {
                model.Data = this.repository
                .GetQuery()
                .FilterUsingAntlr(d => d.Text, input.SearchTerm);
            }
            catch (InvalidSearchException ex)
            {
                ModelState.AddModelError("SearchTerm", ex.Message);

                model.Data = this.repository.GetQuery();
            }

            return View(model);
        }
    }
}