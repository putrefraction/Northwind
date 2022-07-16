using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using Packt.Shared; // NorthwindContext
using Microsoft.AspNetCore.Mvc; // [BindProperty], ActionResult

namespace Northwind.Web.Pages;

public class CustomersModel : PageModel {
    private NorthwindContext db;

    public CustomersModel(NorthwindContext injectedContext){
        db = injectedContext;
    }

    public IEnumerable<Customer>? Customers;

    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Customers";
        Customers = db.Customers.OrderBy(c => c.Country);
    }
}
