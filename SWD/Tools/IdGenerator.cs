using System.Text;

namespace EXE.Tools
{
    public class IdGenerator
    {
        public static string GenerateID()
        {
            // Define the pool of possible characters
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = 6;

            // Create a random number generator
            Random random = new Random();

            // Create a string builder to efficiently build the ID
            StringBuilder sb = new StringBuilder(length);

            // Generate random characters and append them to the string builder
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                char ch = chars[index];
                sb.Append(ch);
            }

            return sb.ToString();
        }

    }
}
