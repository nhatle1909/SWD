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

            [Compare("Password")]
            public required string ConfirmPassword { get; set; }
        }

        public class RegisterForStaffAccountView
        {
            [Required]
            public required string Jwt { get; set; }

            [EmailAddress]
            public required string Email { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }

            [Compare("Password")]
            public required string ConfirmPassword { get; set; }
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
            [Required]
            public required string Jwt { get; set; }

            [EmailAddress]
            public required string Email { get; set; }

            [Phone]
            public string? PhoneNumber { get; set; }

            public string? HomeAdress { get; set; }
        }

        public class UpdatePictureAccountView
        {
            [Required]
            public required string Jwt { get; set; }
            public required IFormFile Picture { get; set; }
        }

        public class ResetPasswordAccountView
        {
            [EmailAddress]
            public required string Email { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }

            [Compare("Password")]
            public required string ConfirmPassword { get; set; }
        }

        public class ChangePasswordAccountView
        {
            [Required]
            public required string Jwt { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string OldPassword { get; set; }

            [StringLength(50, MinimumLength = 5)]
            public required string Password { get; set; }

            [Compare("Password")]
            public required string ConfirmPassword { get; set; }
        }

        public class BanAccountView
        {
            [Required]
            public required string Jwt { get; set; }

            [EmailAddress]
            public required string Email { get; set; }
            public string? Comments { get; set; }
        }

        public class DeleteAccountView
        {
            [Required]
            public required string Jwt { get; set; }

            [EmailAddress]
            public required string Email { get; set; }

            [Required]
            public required string Comments { get; set; }
        }

        public class PagingAccountView
        {
            public required string Jwt { get; set; }
            public int PageIndex { get; set; }
            public bool IsAsc { get; set; }
            public string? SearchValue { get; set; }
        }

        public class DetailAccountView
        {
            public required string Jwt { get; set; }
            [EmailAddress]
            public required string Email { get; set; }
        }
    }
}
