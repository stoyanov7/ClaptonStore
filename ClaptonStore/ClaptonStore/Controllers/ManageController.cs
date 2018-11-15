namespace ClaptonStore.Controllers
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models.BindingModels;
    using Models.Identity;
    using Models.ViewModels;
    using Services;
    using Services.Contracts;
    using Utilities.Constants;

    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;
        private readonly UrlEncoder urlEncoder;

        private const string AuthenicatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        public ManageController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<ManageController> logger,
            UrlEncoder urlEncoder)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.logger = logger;
            this.urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.CheckUser();

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = this.StatusMessage
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.CheckUser();

            if (model.Email != user.Email)
            {
                var setEmailResult = await this.userManager
                    .SetEmailAsync(user, model.Email);

                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException(
                        string.Format(ManageConstants.UnexpectedError, "email", user.Id));
                }
            }

            if (model.PhoneNumber != user.PhoneNumber)
            {
                var setPhoneResult = await this.userManager
                    .SetPhoneNumberAsync(user, model.PhoneNumber);

                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException(
                        string.Format(ManageConstants.UnexpectedError, "phone number", user.Id));
                }
            }

            this.StatusMessage = ManageConstants.UpdateProfile;

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var user = await this.CheckUser();

            var code = await this.userManager
                .GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = this.Url
                .EmailConfirmationLink(user.Id, code, this.Request.Scheme);

            await this.emailSender
                .SendEmailConfirmationAsync(user.Email, callbackUrl);

            this.StatusMessage = ManageConstants.VeritificationEmailSent;

            return this.RedirectToAction(nameof(this.Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await this.CheckUser();

            var hasPassword = await this.userManager
                .HasPasswordAsync(user);

            if (!hasPassword)
            {
                return this.RedirectToAction(nameof(this.SetPassword));
            }

            var model = new ChangePasswordBindingModel { StatusMessage = this.StatusMessage };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var user = await this.CheckUser();

            var changePasswordResult = await this.userManager
                .ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

            if (!changePasswordResult.Succeeded)
            {
                this.AddErrors(changePasswordResult);

                return this.View(model);
            }

            await this.signInManager
                .SignInAsync(user, false);

            this.logger
                .LogInformation("User changed their password successfully.");

            this.StatusMessage = "Your password has been changed.";

            return this.RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await this.CheckUser();

            var hasPassword = await this.userManager
                .HasPasswordAsync(user);

            if (hasPassword)
            {
                return this.RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordBindingModel { StatusMessage = this.StatusMessage };
            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await this.CheckUser();

            var model = new ExternalLoginsViewModel
            {
                CurrentLogins = await this.userManager.GetLoginsAsync(user)
            };

            model.OtherLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();

            model.ShowRemoveButton =
                await this.userManager.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;

            model.StatusMessage = this.StatusMessage;

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = this.Url.Action(nameof(this.LinkLoginCallback));

            var properties = this.signInManager
                .ConfigureExternalAuthenticationProperties(provider, redirectUrl, this.userManager.GetUserId(this.User));

            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await this.CheckUser();

            var info = await this.signInManager
                .GetExternalLoginInfoAsync(user.Id);

            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await this.userManager
                .AddLoginAsync(user, info);

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await this.HttpContext
                .SignOutAsync(IdentityConstants.ExternalScheme);

            this.StatusMessage = "The external login was added.";

            return this.RedirectToAction(nameof(this.ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await this.CheckUser();
            
            var unformattedKey = await this.userManager
                .GetAuthenticatorKeyAsync(user);

            if (string.IsNullOrEmpty(unformattedKey))
            {
                await this.userManager
                    .ResetAuthenticatorKeyAsync(user);

                unformattedKey = await this.userManager.GetAuthenticatorKeyAsync(user);
            }

            var model = new EnableAuthenticatorViewModel
            {
                SharedKey = this.FormatKey(unformattedKey),
                AuthenticatorUri = this.GenerateQrCodeUri(user.Email, unformattedKey)
            };

            return this.View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await this.CheckUser();
            
            var result = await this.userManager
                .RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);

            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await this.signInManager.SignInAsync(user, false);
            this.StatusMessage = "The external login was removed.";

            return this.RedirectToAction(nameof(this.ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await this.CheckUser();

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await this.userManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await this.userManager.CountRecoveryCodesAsync(user),
            };

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await this.CheckUser();

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await this.userManager
                .GenerateNewTwoFactorRecoveryCodesAsync(user, 10);

            var model = new GenerateRecoveryCodesViewModel
            {
                RecoveryCodes = recoveryCodes.ToArray()
            };

            this.logger
                .LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            var user = await this.userManager
                .GetUserAsync(this.User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{userManager.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return this.View(nameof(this.Disable2fa));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await this.userManager
                .GetUserAsync(this.User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var disable2faResult = await this.userManager
                .SetTwoFactorEnabledAsync(user, false);

            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            this.logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);

            return this.RedirectToAction(nameof(this.TwoFactorAuthentication));
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return this.View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.userManager.SetTwoFactorEnabledAsync(user, false);
            await this.userManager.ResetAuthenticatorKeyAsync(user);

            this.logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return this.RedirectToAction(nameof(this.EnableAuthenticator));
        }

        #region Helpers

        /// <summary>
        /// Get the current user if exist.
        /// If user exist returns it. Otherwise throw exception.
        /// </summary>
        /// <exception cref="ApplicationException"></exception>
        /// <returns>Current user if is not null.</returns>
        private async Task<ApplicationUser> CheckUser()
        {
            var user = await this.userManager
                .GetUserAsync(this.User);

            if (user == null)
            {
                var id = this.userManager.GetUserId(this.User);

                throw new ApplicationException(string.Format(ManageConstants.UnalbeToLoadUser, id));
            }

            return user;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            var currentPosition = 0;

            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }

            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenicatorUriFormat,
                this.urlEncoder.Encode("BiraIssueTracker.Web"),
                this.urlEncoder.Encode(email),
                unformattedKey);
        }

        #endregion
    }
}