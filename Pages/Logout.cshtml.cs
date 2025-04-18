using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Week5Lab.Pages
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            
            HttpContext.Session.Clear();

            
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("username");

            
            return RedirectToPage("/Login");
        }
    }
}
