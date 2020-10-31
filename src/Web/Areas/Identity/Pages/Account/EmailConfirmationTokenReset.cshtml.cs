using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class EmailConfirmationTokenReset : PageModel
    {
        
        public void OnGet()
        {

        }
    }
}
