using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5Lab.Models;
using Week5Lab.Data;
using Microsoft.EntityFrameworkCore;



namespace Week5Lab.Pages
{
    public class IndexModel : PageModel
    {
        
        
    
        //public ClassInformationModel NewClass { get; set; } = new();
        [BindProperty]
        public Class NewClass { get; set; } = new Class { ClassName = string.Empty };



        [BindProperty(SupportsGet = true)]
        public int? EditId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ClassNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public const int PageSize = 10;

        public List<ClassInformationTable> DisplayedClasses { get; set; } = new();
        public int TotalPages { get; set; }

        //private static List<ClassInformationModel>? _storage;

        private readonly SchoolDbContext _context;

        public IndexModel(SchoolDbContext context)
        {
            _context = context;
        }

        public IList<Class> ClassList { get; set; } = new List<Class>();

        public async Task OnGetAsync()
                {
                    EnsureSeeded();
                    // ➤ Session kontrolü
                    var sessionToken = HttpContext.Session.GetString("token");
                    var cookieToken = Request.Cookies["token"];
                    var sessionUser = HttpContext.Session.GetString("username");
                    var cookieUser = Request.Cookies["username"];

                    if (string.IsNullOrEmpty(sessionToken) ||
                        string.IsNullOrEmpty(cookieToken) ||
                        sessionToken != cookieToken ||
                        sessionUser != cookieUser)
                    {
                        TempData["Error"] = "You must be logged in.";
                        Response.Redirect("/Login");
                        return;
                    }

                    // ➤ Veri çekme (isteğe bağlı filtreli çekebilirsin)
                    var query = _context.Classes.AsQueryable();
                    query = query.Where(x => x.IsActive); // sadece aktif olanlar


                    if (!string.IsNullOrWhiteSpace(ClassNameFilter))
                    {
                        query = query.Where(x => x.ClassName.Contains(ClassNameFilter));
                    }

                    int totalItems = await query.CountAsync();
                    TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
                    PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

                    ClassList = await query
                        .Skip((PageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync();

                    // ➤ Eğer düzenleme yapılıyorsa, formu doldur
                    if (EditId.HasValue)
                    {
                        var item = await _context.Classes.FindAsync(EditId.Value);
                        if (item != null)
                        {
                            NewClass = item;
                        }
                    }
                }


        

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (EditId.HasValue)
            {
                var existing = await _context.Classes.FindAsync(EditId.Value);
                if (existing != null)
                {
                    existing.ClassName = NewClass.ClassName;
                    existing.StudentCount = NewClass.StudentCount;
                    existing.Description = NewClass.Description;
                    existing.IsActive = NewClass.IsActive;
                }
            }
            else
            {
                _context.Classes.Add(NewClass);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage(new { PageNumber, ClassNameFilter });
        }


        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var item = await _context.Classes.FindAsync(id);
            if (item != null)
            {
                item.IsActive = false;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { PageNumber, ClassNameFilter });
        }


        private List<Class> GenerateFakeData()
        {
            string[] baseNames = { "Math", "Science", "English", "History", "Physics", "Biology", "Art", "Music", "PE", "Chemistry" };
            string[] suffixes = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" }; 


            var list = new List<Class>();
            int idCounter = 1;

            foreach (var name in baseNames)
            {
                foreach (var suffix in suffixes)
                {
                    list.Add(new Class
                    {
                        Id = idCounter,
                        ClassName = $"{name} {suffix}",
                        StudentCount = 15 + (idCounter % 25),
                        Description = $"This is {name} section {suffix}"
                    });
                    idCounter++;
                }
            }

            return list;
        }
        public async Task<IActionResult> OnPostExportAsync(string? selectedColumns, string? FilteredExport)
        {
            List<string>? selectedList = null;

            if (!string.IsNullOrWhiteSpace(selectedColumns))
            {
                selectedList = selectedColumns
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                if (selectedList.Count == 0)
                {
                    selectedList = null;
                }
            }

            var query = _context.Classes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(ClassNameFilter))
            {
                query = query.Where(x => x.ClassName.Contains(ClassNameFilter));
            }

            if (FilteredExport != "true")
            {
                query = query
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize);
            }

            var exportList = await query.ToListAsync();
            var json = Helpers.Utils.Instance.ExportToJson(exportList, selectedList);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var filename = $"classes_{(selectedList == null ? "all" : FilteredExport == "true" ? "filtered" : "page")}_{timestamp}.json";

            return File(System.Text.Encoding.UTF8.GetBytes(json), "application/json", filename);
        }

        public IActionResult OnPostLogout()
            {
                HttpContext.Session.Clear();
                Response.Cookies.Delete("token");
                Response.Cookies.Delete("username");
                Response.Cookies.Delete("session_id");

                return RedirectToPage("/Login");
            }


        private void EnsureSeeded()
        {
            if (!_context.Classes.Any())
            {
                var rnd = new Random();
                for (int i = 1; i <= 100; i++)
                {
                    _context.Classes.Add(new Class
                    {
                        ClassName = $"Class {i}",
                        StudentCount = rnd.Next(10, 50),
                        Description = $"Description for class {i}",
                        IsActive = true
                    });
                }
                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                _context.Users.Add(new User
                {
                    Username = "admin",
                    Password = "1234", // ⚠️ test amaçlı düz şifre
                    IsActive = true
                });

                _context.Users.Add(new User
                {
                    Username = "test",
                    Password = "test",
                    IsActive = true
                });

                _context.SaveChanges();
            }
        }






    }
}
