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

        public static string EncryptEmail(string email)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("9utjwQbRFAVj1Kt5lOVWi9tAwQbRFAVj");//SecretKey
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                byte[] emailBytes = Encoding.UTF8.GetBytes(email);
                byte[] encryptedEmailBytes = encryptor.TransformFinalBlock(emailBytes, 0, emailBytes.Length);

                // Combine IV and encrypted email into one array
                byte[] result = new byte[aesAlg.IV.Length + encryptedEmailBytes.Length];
                Buffer.BlockCopy(aesAlg.IV, 0, result, 0, aesAlg.IV.Length);
                Buffer.BlockCopy(encryptedEmailBytes, 0, result, aesAlg.IV.Length, encryptedEmailBytes.Length);

                return Convert.ToBase64String(result);
            }
        }

        public static string DecryptEmail(string encryptedEmail)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes("9utjwQbRFAVj1Kt5lOVWi9tAwQbRFAVj");//SecretKey

                byte[] fullCipher = Convert.FromBase64String(encryptedEmail);

                // Extract IV from beginning of array
                byte[] iv = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = new byte[fullCipher.Length - iv.Length];
                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] decryptedEmailBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);

                return Encoding.UTF8.GetString(decryptedEmailBytes);
            }
        }


    }
}
