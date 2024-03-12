using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Repositories.ModelView
{
    public class AccountView
    {
        public class RegisterAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }
            public string? PhoneNumber { get; set; }
        }

        public class RegisterForStaffAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }
            public string? PhoneNumber { get; set; }
        }

        public class LoginAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }

            [Required]
            public required string Password { get; set; }
        }

        public class UpdateAccountView
        {
            [Phone]
            public required string PhoneNumber { get; set; }

            public required string HomeAdress { get; set; }
        }

        public class UpdatePictureAccountView
        {
            public required IFormFile Picture { get; set; }
        }

        public class ResetPasswordAccountView
        {
            public required string Token { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }
        }

        public class ChangePasswordAccountView
        {
            [StringLength(50, MinimumLength = 5)]
            public required string OldPassword { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }

            [Compare("Password")]
            public required string ConfirmPassword { get; set; }
        }

        public class BanAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }
            public string? Comments { get; set; }
        }

        public class DeleteAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }

            [Required]
            public required string Comments { get; set; }
        }

        public class PagingAccountView
        {
            public int PageIndex { get; set; }
            public bool IsAsc { get; set; }
            public string? SearchValue { get; set; }
        }

        public class DetailAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }
        }
    }
}
