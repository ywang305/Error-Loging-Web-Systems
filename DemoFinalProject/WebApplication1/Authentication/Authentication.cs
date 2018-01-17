/*

This class handles authentication, taking advantage of the persisting user data class to retrieve/save user data

Recommended reading:
    - discusses differences bettween different hashing methods
      https://stackoverflow.com/questions/800685/which-cryptographic-hash-function-should-i-choose

    - discusses how to do hashing securely
      https://crackstation.net/hashing-security.htm

*/

namespace WebApplication1.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Security.Cryptography;
    using SharedCode;

    public class Authentication
    {
        /// <summary>
        /// This is a SALT.. it should be "random" and complex, while it should not change over time.
        /// 
        /// Best practice is to generate a new salt for each user; in our case we do so by combidning the username w/ the salt, and w/ the password itself
        /// 
        /// This salt, as well as the logic in the HashPassword method should not be made public
        /// </summary>
        private const string SALT = "This.4i35i34890099+IsA Rand0m..<<>>//.,{)_@*$()&%&394802 98S alt390 @&$((*@()\":KFHDDF908r40932i;;efdjsklhfsd8oroy389ryhfsklddnfds218()*$()@^$*()@*YF";

        /// <summary>
        /// Hashes the password for the user, and returns the hashed password
        /// </summary>
        public static string HashPassword(string email, string nonHashedPassword)
        {
            if (string.IsNullOrEmpty(email)) { throw new ArgumentNullException("user.UserName"); }
            if (string.IsNullOrEmpty(nonHashedPassword)) { throw new ArgumentNullException("user.Password"); }

            string result = string.Empty;

            // we are using the SHA512 algorithm. 
            // please read links above to learn about available options, strengths and downsides of different algorithms available
            HashAlgorithm hash = new SHA512CryptoServiceProvider();

            // we break the salt into 3 parts, and then combine it w/ the username as well as the password to make hashtables much more difficult to use
            int thirdOfALength = SALT.Length / 3;

            string stringToHash = SALT.Substring(0, thirdOfALength)
                + email + SALT.Substring(thirdOfALength - 1, thirdOfALength)
                + nonHashedPassword + SALT.Substring(thirdOfALength * 2 - 1);

            // do the hashing (convert string to bytes, do the hashing, convert hashed bytes back to string
            byte[] bytesToHash = Encoding.UTF8.GetBytes(stringToHash);
            byte[] hashedBytes = hash.ComputeHash(bytesToHash);
            result = Convert.ToBase64String(hashedBytes);

            return result;
        }

        /// <summary>
        /// Method authenticates the user.. returns true if the user's credentials are correct, and false if they are not
        /// </summary>
        public static bool AuthenticateUser(string email, string nonHashedPassword)
        {
            bool result = false;

            try
            {
                string hashedPassword = HashPassword(email, nonHashedPassword);

                ICollection<LoginInfoVM> savedUserInfo = LoadersAndLogic.Loader.queryAllLoginPersons();

                // we have to match exactly 1 user item
                result = savedUserInfo.Count(x => x.Password == hashedPassword && x.OnePerson.Email == email) == 1;
            }
            catch
            {
                /* need to log */
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Registers the user if no exist in DB
        /// </summary>
        public static bool RegisterUser(string email, string nonHashedPassword)
        {
            bool result = false;

            try
            {
                string hashedPassword = HashPassword(email, nonHashedPassword);
                ICollection<LoginInfoVM> savedUserInfo = LoadersAndLogic.Loader.queryAllLoginPersons();

                // if there are no existing users already saved.. save it
                if (!savedUserInfo.Any(x => x.Password == hashedPassword && x.OnePerson.Email == email))
                {
                    //savedUserInfo.Add(new UserData() { UserName = email, HashedPassword = hashedPassword });
                    //result = PersistingUserData.SaveUserData(savedUserInfo);
                    result = true;
                }
            }
            catch
            {
                /* need to log */
                result = false;
            }

            return result;
        }
    }
}

