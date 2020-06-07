using OTP.Models;
using System.Collections.Generic;
using System.Linq;

namespace OTP.Services.Repository
{
    /*
     * This is a mock data service that simulates the behiavour of an real repository with a database
     */
    public static class DataService
    {
        private static ICollection<UserModel> UsersMock = new List<UserModel>();

        /*
         * Save the user 
         */
        public static void AddUser(UserModel user)
        {
            UsersMock.Add(user);
        }

        /*
         * "Raw update" of the user
         */
        public static void UpdateUser(UserModel userModel)
        {
            UserModel user = UsersMock.FirstOrDefault(u => u.UserId == userModel.UserId);

            UsersMock.Remove(user);
            UsersMock.Add(userModel);
        }

        /*
         * Get the user by userId
         * Condition: user hasn't logged in yet
         */
        public static UserModel GetUser(int userId)
        {
            UserModel user = UsersMock.FirstOrDefault(u => u.UserId == userId && !u.HasLogged);

            return user;
        }

        /*
         * Check if the user exists
         * Condition: user hasn't logged in yet 
         */
        public static bool CheckUserById(int userId)
        {
            bool exists = UsersMock.Any(u => u.UserId == userId && !u.HasLogged);

            return exists;
        }
    }
}