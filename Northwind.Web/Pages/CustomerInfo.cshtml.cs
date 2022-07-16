using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using Packt.Shared; // NorthwindContext
using Microsoft.AspNetCore.Mvc; // [BindProperty], ActionResult

namespace Northwind.Web.Pages;

public class CustomerInfoModel : PageModel {
    private NorthwindContext db;

    public CustomerInfoModel(NorthwindContext injectedContext){
        db = injectedContext;
    }

    public IEnumerable<Order> Orders;
    public Customer? Customer;

    public void OnGet(string id)
    {
        ViewData["Id"] = "Northwind B2B - Customers";
        Customer = db.Customers.Find(id);
        
        if(Customer is not null)
        {
            Orders = db.Orders.Where(o => o.CustomerId == id);
        }

    }
}
