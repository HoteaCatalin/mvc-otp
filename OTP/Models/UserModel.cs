namespace OTP.Models
{
    public class UserModel : BaseModel
    {
        public int UserId { get; set; }

        public string Password { get; set; }

        public bool HasLogged { get; set; }
    }
}