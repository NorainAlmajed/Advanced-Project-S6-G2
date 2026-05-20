using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdvancedProjectAPI.Data;      
using AdvancedProjectAPI.Dtos;     
using AdvancedProjectAPI.Models;   
using AdvancedProjectAPI.Services; 

namespace AdvancedProjectAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var existing = await _userManager.FindByEmailAsync(dto.Email);
        if (existing != null)
            return Conflict(new { message = "Email already registered." });

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FullName = dto.FullName  // Changed from DisplayName
        };

        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
            return BadRequest(new
            {
                errors = result.Errors.Select(e => e.Description)
            });

        // New users get Tenant role by default
        await _userManager.AddToRoleAsync(user, IdentitySeeder.TenantRole);

        return await BuildAuthResponseAsync(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });

        var result = await _signInManager.CheckPasswordSignInAsync(
            user, dto.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });

        return await BuildAuthResponseAsync(user);
    }

    private async Task<AuthResponseDto> BuildAuthResponseAsync(
        ApplicationUser user)
    {
        var token = await _tokenService.CreateTokenAsync(user);
        var expiryMinutes = int.Parse(_config["Jwt:ExpiryMinutes"]!);
        var roles = await _userManager.GetRolesAsync(user);

        return new AuthResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Email = user.Email!,
            FullName = !string.IsNullOrEmpty(user.FullName)  
                ? user.FullName
                : user.UserName!,
            Roles = roles
        };
    }
}