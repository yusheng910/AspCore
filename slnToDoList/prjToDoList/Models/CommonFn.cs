using System.Security.Cryptography;
using System.Text;

namespace prjToDoList.Models
{
    public class CommonFn
    {
        // Password SHA256 encryption
        public static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    stringBuilder.Append(hashBytes[i].ToString("x2")); // Convert byte to hexadecimal string
                }

                return stringBuilder.ToString();
            }
        }
    }
}
