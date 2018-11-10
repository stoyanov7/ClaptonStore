namespace ClaptonStore.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class ResetPasswordBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(PasswordUtility.MaxLength,
            ErrorMessage = PasswordUtility.ErrorMessage,
            MinimumLength = PasswordUtility.MinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = PasswordUtility.ConfirmationMessage)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}