using System.Security.Cryptography;
using System.Text;

namespace Learn2.Controllers {
    public class PasswordHelper {

        public static string HashPassword(string password) {
            using (SHA256 sha256Hash = SHA256.Create()) {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string generateAPIKey() {
            const string valid = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for(int i = 0; i < 16; i++) {
                res.Append(rnd.Next(valid.Length));
            }
            return res.ToString();
        }
    }
}
