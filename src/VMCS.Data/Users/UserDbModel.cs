using System.ComponentModel.DataAnnotations.Schema;

namespace VMCS.Data.Users
{
    [Table("user")]
    public class UserDbModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        
    }
}
