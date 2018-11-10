namespace ClaptonStore.Models.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class ForgotPasswordBindingModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}