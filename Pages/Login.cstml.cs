using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Week5Lab.Models;

namespace Week5Lab.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = "";

        [BindProperty]
        public string Password { get; set; } = "";

        public string? ErrorMessage { get; set; }

        public IActionResult OnPost()
        {
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "users.json");

            if (!System.IO.File.Exists(jsonPath))
            {
                ErrorMessage = "User data not found.";
                return Page();
            }

            var json = System.IO.File.ReadAllText(jsonPath);
            var users = JsonSerializer.Deserialize<List<User>>(json);

            var user = users?.FirstOrDefault(u => 
                u.Username == Username && 
                u.Password == Password && 
                u.IsActive);

            if (user != null)
            {
                           
                string token = Guid.NewGuid().ToString(); 

                
                HttpContext.Session.SetString("username", user.Username);
                HttpContext.Session.SetString("token", token);
                HttpContext.Session.SetString("session_id", HttpContext.Session.Id);

                
                CookieOptions cookieOptions = new()
                {
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                };

                Response.Cookies.Append("username", user.Username, cookieOptions);
                Response.Cookies.Append("token", token, cookieOptions);
                Response.Cookies.Append("session_id", HttpContext.Session.Id, cookieOptions);

                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Username or password is incorrect.";
                return Page();
            }
        }
    }
}
