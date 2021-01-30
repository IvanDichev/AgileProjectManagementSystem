using Data.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Utilities.Mailing.SendGrid;

namespace Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetEmailConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IConfiguration _config;

        public ResetEmailConfirmationModel(
            UserManager<User> userManager,
            ILogger<RegisterModel> logger,
            IConfiguration config)
        {
            _userManager = userManager;
            _logger = logger;
            _config = config;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user != null && user.EmailConfirmed == false)
                {
                    _logger.LogInformation($"User with id {user.Id} got reset of the email confirmation token.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code},
                        protocol: Request.Scheme);

                    var emailSender = new SendGridEmailSender(this._config["SendGrid:Key"]);
                    await emailSender.SendEmailAsync(_config["SendGrid:Email"], EmailConstants.FromMailingName, Input.Email,
                       EmailConstants.ConfirmationEmailSubject,
                       string.Format(EmailConstants.ConfirmEmail, HtmlEncoder.Default.Encode(callbackUrl)));
                }
            }
            return RedirectToPage("./EmailConfirmationTokenReset");
        }
    }
}
