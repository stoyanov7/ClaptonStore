namespace ClaptonStore.Models.ViewModels
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Utilities;

    public class EnableAuthenticatorViewModel
    {
        [Required]
        [StringLength(1, 
            ErrorMessage = PasswordUtility.ErrorMessage, 
            MinimumLength = PasswordUtility.MinLength)]
        [DataType(DataType.Text)]
        [Display(Name = "Verification Code")]
        public string Code { get; set; }

        [ReadOnly(true)]
        public string SharedKey { get; set; }

        public string AuthenticatorUri { get; set; }
    }
}