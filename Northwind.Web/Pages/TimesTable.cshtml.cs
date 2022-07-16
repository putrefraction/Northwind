using Microsoft.AspNetCore.Mvc.RazorPages; // PageModel
using Microsoft.AspNetCore.Mvc; // [BindProperty], ActionResult

namespace Northwind.Web.Pages;

// public class TimesTableModel : PageModel
// {
//     public int Value { get; set; }
//     public List<string> Table { get; set; } = null!;

//     public void OnGet()
//     {
//         ViewData["Title"] = "Northwind Utils - Times Table";
//     }

//     public IActionResult OnPost(int value)
//     {
//         Value = value;
//         for (int i = 1; i <= 10; i++)
//         {
//             int temp = i * value;
//             string result = $"{value} x {i} = {temp}";
//             Table.Add(result);
//         }

//         return Page();
//     }
// }