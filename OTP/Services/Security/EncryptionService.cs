using OTP.Helper;
using System;
using System.Security.Cryptography;
using System.Text;

namespace OTP.Services.Security
{
    public static class EncryptionService
    {
        private static readonly DateTime UnixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string GetPassword(string userId)
        {
            long iteration = (long)(DateTime.UtcNow - UnixDateTime).TotalSeconds / 30;

            return GeneratePassword(userId, iteration);
        }

        /*
         * Generate password from userId and current dateTime
         */
        public static string GeneratePassword(string userId, long iteration)
        {
            byte[] iterationByte = BitConverter.GetBytes(iteration);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(iterationByte); //To BigEndian
            }

            //Hash the userId by HMAC-SHA-1
            byte[] userIdByte = Encoding.ASCII.GetBytes(userId);
            HMACSHA1 userIdHMAC = new HMACSHA1(userIdByte, true);
            byte[] hash = userIdHMAC.ComputeHash(iterationByte); //Hashing a message with a secret key

            //RFC4226 http://tools.ietf.org/html/rfc4226#section-5.4
            int offset = hash[hash.Length - 1] & 0xf; //0xf = 15d
            int binary =
                ((hash[offset] & 0x7f) << 24)      //0x7f = 127d
                | ((hash[offset + 1] & 0xff) << 16) //0xff = 255d
                | ((hash[offset + 2] & 0xff) << 8)
                | (hash[offset + 3] & 0xff);

            int password = binary % (int)Math.Pow(10, Constants.PasswordDigits); // Shrink the password to desired length
            return password.ToString(new string('0', Constants.PasswordDigits));
        }
    }
}