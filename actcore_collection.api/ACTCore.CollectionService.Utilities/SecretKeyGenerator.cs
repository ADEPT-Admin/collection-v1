using System.Security.Cryptography;
using System.Text;

namespace ACTCore.CollectionService.Utilities
{
    public class SecretKeyGenerator
    {
        private readonly byte[] aesKey;
        private readonly byte[] aesIV;

        public SecretKeyGenerator(string masterKey)
        {
            // ใช้ SHA256 เพื่อสร้าง key และ IV จาก master key
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(masterKey));
            aesKey = new byte[16];
            aesIV = new byte[16];
            Array.Copy(hash, 0, aesKey, 0, 16);
            Array.Copy(hash, 16, aesIV, 0, 16);
        }

        public string GenerateSecretKey(string keyword, string userId, DateTime? expiryDate = null)
        {
            var data = new StringBuilder();
            data.Append($"KEYWORD:{keyword};");
            data.Append($"USERID:{userId};");
            data.Append($"EXPIRY:{(expiryDate ?? DateTime.MinValue):yyyy-MM-dd};");
            data.Append($"TIMESTAMP:{DateTime.Now:yyyyMMddHHmmss};");
            data.Append($"RAND:{Guid.NewGuid()}");

            string plainText = data.ToString();

            using var aes = Aes.Create();
            aes.Key = aesKey;
            aes.IV = aesIV;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs))
            {
                writer.Write(plainText);
            }

            return Convert.ToBase64String(ms.ToArray());
        }

        public Dictionary<string, string> ExtractData(string secretKey)
        {
            try
            {
                var cipherBytes = Convert.FromBase64String(secretKey);

                using var aes = Aes.Create();
                aes.Key = aesKey;
                aes.IV = aesIV;

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using var ms = new MemoryStream(cipherBytes);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using var reader = new StreamReader(cs);
                var plainText = reader.ReadToEnd();

                var result = new Dictionary<string, string>();
                var parts = plainText.Split(';');
                foreach (var part in parts)
                {
                    var kv = part.Split(':', 2);
                    if (kv.Length == 2)
                        result[kv[0]] = kv[1];
                }

                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}
