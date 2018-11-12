namespace ClaptonStore.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class RegisterBindingModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(
            PasswordUtility.MaxLength,
            ErrorMessage = PasswordUtility.ErrorMessage,
            MinimumLength = PasswordUtility.MinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = PasswordUtility.ConfirmationMessage)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}