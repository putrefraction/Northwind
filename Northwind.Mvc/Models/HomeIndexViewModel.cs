using Packt.Shared; /* Category, Product */

namespace Northwind.Mvc.Models;

public record HomeIndexViewModel(
    int VisitorCount,
    IList<Product> Products,
    IList<Category> Categories
);