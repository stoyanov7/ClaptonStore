namespace ClaptonStore.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Identity;
    using Models.ViewModels;
    using Services;
    using Services.Contracts;
    using Utilities.Constants;

    public class ManageController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEmailSender emailSender;

        public ManageController(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await this.userManager
                .GetUserAsync(this.User);

            if (user == null)
            {
                throw new ApplicationException(
                    string.Format(ManageConstants.UnalbeToLoadUser, 
                        this.userManager.GetUserId(this.User)));
            }

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

            var user = await this.userManager
                .GetUserAsync(this.User);

            if (user == null)
            {
                throw new ApplicationException(
                    string.Format(ManageConstants.UnalbeToLoadUser,
                        this.userManager.GetUserId(this.User)));
            }

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

            var user = await this.userManager
                .GetUserAsync(this.User);

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            var code = await this.userManager
                .GenerateEmailConfirmationTokenAsync(user);

            var callbackUrl = this.Url
                .EmailConfirmationLink(user.Id, code, this.Request.Scheme);

            await this.emailSender
                .SendEmailConfirmationAsync(user.Email, callbackUrl);

            this.StatusMessage = "Verification email sent. Please check your email.";

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}