namespace VMCS.API.Controllers.Users.Dto
{
    public class CreateUserDto
    {
        public string Login { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
