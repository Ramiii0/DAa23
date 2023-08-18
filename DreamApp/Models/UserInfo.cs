using System.ComponentModel.DataAnnotations;

namespace WbToken
{
    public class UserInfo
    {
        [Key]
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
