using System.Security.Cryptography;
using System.Text;
namespace Services.Tool
{
    public class SomeTool
    {
        public static byte[]? GetImage(string base64String)
        {
            byte[]? bytes = null;
            if (!string.IsNullOrEmpty(base64String)) 
                bytes = Convert.FromBase64String(base64String);
            return bytes;
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
