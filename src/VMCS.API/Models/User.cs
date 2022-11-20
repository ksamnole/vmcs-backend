using System;
using Microsoft.AspNetCore.Identity;

namespace VMCS.API.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }
        public string Username { get; set; }
        public Guid Id { get; set; } 
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
