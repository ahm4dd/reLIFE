using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace reLIFE.BusinessLogic.Security
{
    public class PasswordHasher
    {
        private const int SaltSize = 16; // 16 bytes = 128 bits
        private const int HashSize = 32; // 32 bytes = 256 bits (for SHA256)

        /// <summary>
        /// Hashes a password using SHA256 with a unique salt.
        /// </summary>
        /// <param name="password">The plain text password to hash.</param>
        /// <param name="salt">Output: The generated salt (as a hex string).</param>
        /// <returns>The computed hash (as a hex string).</returns>
        public string HashPassword(string password, out string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            // Generate a random salt
            byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);

            // Convert password string to bytes (using UTF-8 encoding)
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Combine password and salt bytes
            byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

            // Compute the SHA256 hash
            byte[] hashBytes = SHA256.HashData(combinedBytes);

            // Convert salt and hash to hex strings for storage
            salt = Convert.ToHexString(saltBytes);
            return Convert.ToHexString(hashBytes);
        }

        /// <summary>
        /// Verifies a password against a stored hash and salt.
        /// </summary>
        /// <param name="password">The plain text password to verify.</param>
        /// <param name="storedHash">The stored hash (as a hex string).</param>
        /// <param name="storedSalt">The stored salt (as a hex string).</param>
        /// <returns>True if the password matches the hash; otherwise, false.</returns>
        public bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash) || string.IsNullOrEmpty(storedSalt))
            {
                return false; // Or throw ArgumentNullException depending on desired strictness
            }

            try
            {
                // Convert stored hex strings back to bytes
                byte[] saltBytes = Convert.FromHexString(storedSalt);
                byte[] expectedHashBytes = Convert.FromHexString(storedHash);

                // Basic validation
                if (saltBytes.Length != SaltSize || expectedHashBytes.Length != HashSize)
                {
                    // Log this potential issue: Invalid hash/salt format retrieved
                    Console.WriteLine("Warning: Stored hash or salt has unexpected length.");
                    return false;
                }

                // Convert input password string to bytes
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Combine input password and stored salt bytes
                byte[] combinedBytes = new byte[saltBytes.Length + passwordBytes.Length];
                Buffer.BlockCopy(saltBytes, 0, combinedBytes, 0, saltBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, combinedBytes, saltBytes.Length, passwordBytes.Length);

                // Compute the hash of the input password and stored salt
                byte[] actualHashBytes = SHA256.HashData(combinedBytes);

                // Compare the computed hash with the stored hash securely
                // CryptographicOperations.FixedTimeEquals is important to prevent timing attacks
                return CryptographicOperations.FixedTimeEquals(actualHashBytes, expectedHashBytes);
            }
            catch (FormatException ex) // Catch errors from Convert.FromHexString
            {
                // Log this potential issue: Stored hash/salt is not valid hex
                Console.WriteLine($"Error decoding hex string: {ex.Message}");
                return false;
            }
            catch (Exception ex) // Catch unexpected errors
            {
                Console.WriteLine($"Error during password verification: {ex.Message}");
                return false; // Treat unexpected errors as verification failure for security
            }
        }
    }
}