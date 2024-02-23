using System.Security.Cryptography;
using System.Text;
namespace Repository.Tools
{
    public class IdGenerator
    {
        public static string GenerateID()
        {
            // Define the pool of possible characters
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = 6;

            // Create a random number generator
            Random random = new();

            // Create a string builder to efficiently build the ID
            StringBuilder sb = new(length);

            // Generate random characters and append them to the string builder
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                char ch = chars[index];
                sb.Append(ch);
            }

            return sb.ToString();
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
