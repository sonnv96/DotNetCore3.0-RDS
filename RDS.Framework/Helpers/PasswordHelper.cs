using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RDS.Framework.Helpers
{
    public static class PasswordHelper
    {
        public static string GenerateSaltPassword()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static string GenerateHashPassword(string password, string saltPassword)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = Convert.FromBase64String(saltPassword);

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public static bool VerifyHashedPassword(string passwordParams, string password, string saltPassword)
        {
            var verifyPassword = GenerateHashPassword(passwordParams, saltPassword);
            return verifyPassword == password;
        }
    }
}
