using GenericSearch.Core;
using GenericSearch.Data;
using GenericSearch.Paging;
using GenericSearch.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace GenericSearch.UI.Controllers;

public class PagedSearchController : Controller
{
    private readonly Repository repository;

    public PagedSearchController()
    {
        this.repository = new Repository();
    }

    public ActionResult Index(Paging<SomeClass> paging, ICollection<AbstractSearch> searchCriteria)
    {
        if (searchCriteria == null || searchCriteria.Count == 0)
        {
            searchCriteria = typeof(SomeClass)
                .GetDefaultSearchCriteria();
        }

        paging.Top = 5;

        var data = this.repository
            .GetQuery()
            .ApplySearchCriteria(searchCriteria)
            .GetPagedResult(paging);

        var model = new PagedSearchViewModel(data, searchCriteria);

        return this.View(model);
    }
}
