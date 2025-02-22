using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages
{
    public class SearchModel : PageModel
    {
        private readonly PRN221DBContext _db;

        public SearchModel(PRN221DBContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<Product> products { get; set; } = new List<Product>();

        [BindProperty]
        public int page { get; set; }

        [BindProperty]
        public int numberPage { get; set; }

        public class SearchCondition
        {
            public int pageIndex { get; set; } = 1;
            public decimal? min { get; set; }
            public decimal? max { get; set; }
            public int? categoryId { get; set; }
        }

        [BindProperty]
        public SearchCondition search { get; set; } = new SearchCondition();

        [BindProperty]
        public string url { get; set; }

        [BindProperty]
        public List<Category> categories { get; set; }
        public async Task<IActionResult> OnGetAsync(SearchCondition search)
        {
            categories =_db.Categories.ToList();
            ViewData["Category"] = new SelectList(_db.Categories, "CategoryId", "CategoryName");
            int pageSize = 8;
            page = search.pageIndex;
            int total = _db.Products
                .Where(x => x.Discontinued == false
                         && (search.min == null || x.UnitPrice >= search.min)
                         && (search.max == null || x.UnitPrice <= search.max)
                         && (search.categoryId == null || x.CategoryId == search.categoryId))
                .Count();
            numberPage = (int)Math.Ceiling((double)total / (double)pageSize);
            products = _db.Products
                .Include(x => x.Category)
                .Where(x => x.Discontinued == false
                         && (search.min == null || x.UnitPrice >= search.min)
                         && (search.max == null || x.UnitPrice <= search.max)
                         && (search.categoryId == null || x.CategoryId == search.categoryId))
                .Skip((search.pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            url = "Search?categoryId=" + search.categoryId + "&min=" + search.min + "&max=" + search.max + "&pageIndex=";
            return Page();
        }
    }
}
