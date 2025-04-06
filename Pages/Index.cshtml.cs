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

        public List<ClassInformationModel> ClassList { get; set; } = new();

        private static List<ClassInformationModel> _storage = new();

        public void OnGet()
        {
            ClassList = _storage;

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
            else
            {
                NewClass = new ClassInformationModel();
            }
        }

        public IActionResult OnPost()
        {
            ClassList = _storage;

            if (!ModelState.IsValid)
                return Page();

            if (EditId.HasValue)
            {
                // Düzenleme
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
                // Yeni Ekleme → En küçük boş ID'yi bul
                int newId = 1;
                while (_storage.Any(x => x.Id == newId))
                    newId++;

                NewClass.Id = newId;
                _storage.Add(NewClass);
            }

            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int id)
        {
            var item = _storage.FirstOrDefault(c => c.Id == id);
            if (item != null)
                _storage.Remove(item);

            return RedirectToPage();
        }
    }
}
