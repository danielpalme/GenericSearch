using System.Linq;
using GenericSearch.Common;
using GenericSearch.Data;
using GenericSearch.Grammar;
using GenericSearch.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.UI.Controllers
{
    public class GrammarSearchController : Controller
    {
        private readonly Repository repository;

        public GrammarSearchController()
        {
            this.repository = new Repository();
        }

        public ActionResult Index(GrammarSearchViewModel model)
        {
            try
            {
                var data = this.FilterUsingGrammar(model);
                model.Data = data;
                model.Terms = data.Terms;
            }
            catch (InvalidSearchException ex)
            {
                ModelState.AddModelError("SearchTerm", ex.Message);

                model.Data = this.repository.GetQuery();
                model.Terms = Enumerable.Empty<string>();
            }

            return this.View(model);
        }

        private SearchResult<SomeClass> FilterUsingGrammar(GrammarSearchViewModel model)
        {
            if (model.Grammar == Models.Grammar.Irony)
            {
                return this.repository
                    .GetQuery()
                    .FilterUsingIrony(model.SearchTerm, m => m.Text);
            }
            else
            {
                return this.repository
                    .GetQuery()
                    .FilterUsingAntlr(model.SearchTerm, m => m.Text);
            }
        }
    }
}