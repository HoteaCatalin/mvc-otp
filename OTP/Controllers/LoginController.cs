using Microsoft.AspNetCore.Mvc;
using OTP.Helper;
using OTP.Models;
using OTP.Services.DataAccess;
using OTP.Services.Security;
using System;

namespace OTP.Controllers
{
    /*
     * Controller that handles the one-time password generation and login
    */
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /*
         * Generate one-time password for the user 
         */
        [HttpPost]
        public ActionResult GetUserOTP(int userId)
        {
            if (userId == 0)
            {
                ViewData["Status"] = ErrorCodes.InvalidUserId;
                return View("UserLogin", new UserModel { UserId = userId });
            }

            var userExists = DataService.CheckUserById(userId);

            if (userExists)
            {
                ViewData["Status"] = ErrorCodes.PasswordNotUsed;
                return View("UserLogin", new UserModel { UserId = userId });
            }

            if (int.TryParse(EncryptionService.GetPassword(userId.ToString()), out int otp)) // if password generation is successful
            {
                var userModel = new UserModel
                {
                    UserId = userId,
                    Password = otp.ToString(),
                    DateCreated = DateTime.UtcNow,
                    HasLogged = false
                };

                DataService.AddUser(userModel);

                ViewData["User"] = userModel.UserId;
                ViewData["Status"] = ErrorCodes.PasswordWarning + otp;
                return View("UserLogin", userModel);
            }
            else
            {
                ViewData["Status"] = ErrorCodes.PasswordCreationFailed;
                return View();
            }
        }

        /*
         * Performs the login action
         * Checks for validity of credentials, time passed from password generation and one-time use of the password
         */
        [HttpPost]
        public ActionResult Login(int userId, string password)
        {
            var date = DateTime.UtcNow;
            var userModel = DataService.GetUser(userId);

            if (password == userModel.Password && userId == userModel.UserId)
            {
                double timeDifference = Math.Abs((DateTime.UtcNow - userModel.DateCreated).TotalSeconds); //seconds elapsed since password was created
                if (timeDifference < Constants.PasswordValidity && !userModel.HasLogged)
                {
                    userModel.HasLogged = true;
                    DataService.UpdateUser(userModel);
                    return View("../UserArea/Account/AccountDetails", userModel);
                }
                else
                {
                    ViewData["Status"] = ErrorCodes.PasswordNotValid;
                    return View("UserLogin", userModel);
                }
            }

            ViewData["Status"] = ErrorCodes.InvalidCredentials;
            return View("UserLogin", new UserModel { UserId = userId });
        }
    }
}