using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VMCS.API.Models;
using VMCS.Core.Domains.Auth;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Services;

namespace VMCS.API.Controllers.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly SignInManager<AuthUser> _signInManager;
    private readonly UserManager<AuthUser> _userManager;
    private readonly IUserService _userService;

    public AuthController
    (UserManager<AuthUser> userManager,
        SignInManager<AuthUser> signInManager,
        IUserService userService,
        IConfiguration configuration)
    {
        _userService = userService;
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
    }

    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(RegisterDTO registerData, CancellationToken cancellationToken)
    {
        // Не менять! тут так и надо, потому что UserName Это Login в IdentityUser
        var user = new AuthUser
        {
            UserName = registerData.Login,
            Email = registerData.Email,
            GivenName = registerData.Username
        };

        var businessUser = new User
        {
            Id = user.Id,
            Login = registerData.Login,
            Username = registerData.Username,
            Email = registerData.Email
        };

        await _userService.Create(businessUser, cancellationToken);

        var result = await _userManager.CreateAsync(user, registerData.Password);

        if (!result.Succeeded) await _userService.Delete(businessUser.Id, cancellationToken);

        if (result.Succeeded)
        {
            var claims = await GetUserClaims(user);

            var token = GetToken(claims);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
        }

        var errors = string.Join(";\n", result.Errors.Select(error => error.Description));
        return BadRequest(errors);
    }

    [Route("login")]
    [HttpPost]
    public async Task<IActionResult> Login(LoginDTO loginData)
    {
        var user = await _userManager.FindByNameAsync(loginData.Login);
        if (user == null)
            return BadRequest("Wrong credentials");
        if (!await _userManager.CheckPasswordAsync(user, loginData.Password))
            return BadRequest("Wrong credentials");

        var claims = await GetUserClaims(user);

        var token = GetToken(claims);

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), expiration = token.ValidTo });
    }

    [Route("logout")]
    [HttpGet]
    public async Task Logout()
    {
        await _signInManager.SignOutAsync();
    }

    [Route("whoami")]
    [HttpGet]
    public async Task WhoAmI()
    {
        var whoami = HttpContext.User.Identity?.Name ?? "Anonymous";
        await HttpContext.Response.WriteAsync(whoami);
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            _configuration["JWT:ValidIssuer"],
            _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(24),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    private async Task<List<Claim>> GetUserClaims(AuthUser user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.GivenName, user.GivenName),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        return claims.Concat(await _userManager.GetClaimsAsync(user)).ToList();
    }
}