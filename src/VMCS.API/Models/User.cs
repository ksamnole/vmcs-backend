using System;
using Microsoft.AspNetCore.Identity;

namespace VMCS.API.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }

        public User() { }
        
        public User(RegisterDTO registerData)
        {
            Login = registerData.Login;
            base.UserName = registerData.Username;
            base.Email = registerData.Email;
        }
    }
}
