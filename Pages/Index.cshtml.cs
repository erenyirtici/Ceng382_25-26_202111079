using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Week5Lab.Models;

namespace Week5Lab.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public ClassInformationModel NewClass { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int? EditId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ClassNameFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public const int PageSize = 10;

        public List<ClassInformationTable> DisplayedClasses { get; set; } = new();
        public int TotalPages { get; set; }

        private static List<ClassInformationModel>? _storage;

        public void OnGet()
        {
            if (_storage == null || !_storage.Any())
            {
                _storage = GenerateFakeData(); 
            }

            if (EditId.HasValue)
            {
                var item = _storage.FirstOrDefault(c => c.Id == EditId.Value);
                if (item != null)
                {
                    NewClass = new ClassInformationModel
                    {
                        Id = item.Id,
                        ClassName = item.ClassName,
                        StudentCount = item.StudentCount,
                        Description = item.Description
                    };
                }
            }

            var query = _storage.AsQueryable();

            if (!string.IsNullOrWhiteSpace(ClassNameFilter))
            {
                query = query.Where(x => x.ClassName.Contains(ClassNameFilter, StringComparison.OrdinalIgnoreCase));
            }

            int totalItems = query.Count();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            PageNumber = Math.Clamp(PageNumber, 1, Math.Max(1, TotalPages));

            DisplayedClasses = query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .Select(x => new ClassInformationTable
                {
                    Id = x.Id,
                    ClassName = x.ClassName,
                    StudentCount = x.StudentCount,
                    Description = x.Description
                })
                .ToList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid || _storage == null)
                return Page();

            if (EditId.HasValue)
            {
                var existing = _storage.FirstOrDefault(x => x.Id == EditId.Value);
                if (existing != null)
                {
                    existing.ClassName = NewClass.ClassName;
                    existing.StudentCount = NewClass.StudentCount;
                    existing.Description = NewClass.Description;
                }
            }
            else
            {
                int newId = 1;
                while (_storage.Any(x => x.Id == newId))
                    newId++;

                NewClass.Id = newId;
                _storage.Add(NewClass);
            }

            return RedirectToPage(new { PageNumber, ClassNameFilter });
        }

        public IActionResult OnPostDelete(int id)
        {
            if (_storage == null) return RedirectToPage();

            var item = _storage.FirstOrDefault(c => c.Id == id);
            if (item != null)
                _storage.Remove(item);

            return RedirectToPage(new { PageNumber, ClassNameFilter });
        }

        private static List<ClassInformationModel> GenerateFakeData()
        {
            string[] baseNames = { "Math", "Science", "English", "History", "Physics", "Biology", "Art", "Music", "PE", "Chemistry" };
            string[] suffixes = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" }; 


            var list = new List<ClassInformationModel>();
            int idCounter = 1;

            foreach (var name in baseNames)
            {
                foreach (var suffix in suffixes)
                {
                    list.Add(new ClassInformationModel
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
    }
}
