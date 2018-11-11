namespace ClaptonStore.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(PasswordUtility.MaxLength,
            ErrorMessage = PasswordUtility.ErrorMessage, 
            MinimumLength = PasswordUtility.MinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", 
            ErrorMessage = PasswordUtility.ConfirmationMessage)]
        public string ConfirmPassword { get; set; }

        public string StatusMessage { get; set; }
    }
}