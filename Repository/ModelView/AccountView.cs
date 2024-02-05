namespace Repository.ModelView
{
    public class AccountView
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
        public required bool isBanned { get; set; }

    }
}
