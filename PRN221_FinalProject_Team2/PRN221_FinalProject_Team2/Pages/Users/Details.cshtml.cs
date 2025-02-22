using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRN221_FinalProject_Team2.Models;

namespace PRN221_FinalProject_Team2.Pages.Users
{
    public class DetailsModel : PageModel
    {

        private readonly PRN221_FinalProject_Team2.Models.PRN221DBContext _db;

        public DetailsModel(PRN221_FinalProject_Team2.Models.PRN221DBContext db)
        {
            _db = db;
        }

        public Account Account { get; set; } = default;

       public async Task<IActionResult> OnGetAync(int? AccountID)
        {
            if (AccountID == null || _db.Accounts == null) 
            {
                return NotFound();
            }

            var account = await _db.Accounts.FirstOrDefaultAsync(a => a.AccountId == AccountID);
            if(account == null) 
            {
                return NotFound();
            }else
            {
                Account= account;
            }
            return Page();
        }
    }
}
