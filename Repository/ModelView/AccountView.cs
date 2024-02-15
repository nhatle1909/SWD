using System.ComponentModel.DataAnnotations;

namespace Repository.ModelView
{
    public class AccountView
    {

        [StringLength(50, MinimumLength = 5)] public required string Password { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Mail")] public required string Email { get; set; }

        [Phone(ErrorMessage = "Invalid Phone Number")] public required string PhoneNumber { get; set; }
        public required string Address { get; set; }

    }
}
