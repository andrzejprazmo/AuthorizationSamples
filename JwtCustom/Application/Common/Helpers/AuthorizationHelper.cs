using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace JwtCustom.Application.Common.Helpers
{
    public class AuthorizationHelper
    {
        public static string EncryptPassword(Guid userId, string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: userId.ToByteArray(),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }
    }
}
