using System;

namespace MongoDbImportTool.Utilities
{
    /// <summary>
    /// Provides a collection of methods for encrypting/decrypting tokens and secrets
    /// </summary>
    public static class EncryptionUtility
    {
        /// <summary>
        /// Generates a time-based access token for checking against idle users
        /// </summary>
        public static string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            return Convert.ToBase64String(time);
        }

        /// <summary>
        /// Encrypts a password for storage
        /// </summary>
        /// <param name="secret">The secret to hash</param>
        public static string HashSecret(string secret)
        {
            return string.IsNullOrWhiteSpace(secret)
                ? null
                : BCrypt.Net.BCrypt.HashPassword(secret);
        }
    }
}
