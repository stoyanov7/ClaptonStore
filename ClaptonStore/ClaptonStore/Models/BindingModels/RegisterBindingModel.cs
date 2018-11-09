namespace ClaptonStore.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class RegisterBindingModel
    {
        [Required]
        [MinLength(3)]
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
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}