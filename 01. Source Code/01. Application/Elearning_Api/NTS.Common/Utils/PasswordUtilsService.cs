using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NTS.Common.Utils
{
    public class PasswordUtilsService : IPasswordUtilsService
    {
        public string CreatePasswordHash()
        {
            string passwordHash = string.Empty;
            Random random = new Random();
            while (passwordHash.Length < 32)
            {
                passwordHash = passwordHash + Convert.ToString(random.Next(0, 9));
            }
            return passwordHash;
        }

        /// <summary>
        /// Hàm tạo chuỗi SecurityStamp
        /// </summary>
        /// <param name="target">chuỗi PasswordHash để tạo chuỗi SecurityStamp</param>
        /// <returns></returns>
        public string ComputeHash(string target)
        {
            SHA256Managed hashAlgorithm = new SHA256Managed();

            byte[] data = System.Text.Encoding.ASCII.GetBytes(target);

            byte[] bytes = hashAlgorithm.ComputeHash(data);

            return BitConverter.ToString(bytes).ToLower().Replace("-", string.Empty);
        }
    }
}
