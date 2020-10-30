using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class EmailNotConfimed : PageModel
    {
        public void OnGet()
        {

        }
        public void OnPost()
        {
            //TODO
        }
    }
}
