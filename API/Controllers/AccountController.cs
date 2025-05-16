namespace API.Controllers;

using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using API.Interfaces;

public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    IMapper mapper) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync(RegisterRequest request)
    {
        if (await UserExistsAsync(request.username))
        {
            return BadRequest("Username already in use");
        }

        var user = mapper.Map<AppUser>(request);
        user.UserName = request.username.ToLowerInvariant();
        var result = await userManager.CreateAsync(user, request.password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return new UserResponse
        {
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginAsync(LoginRequest request)
    {
        var user = await userManager.Users
            .Include(x => x.Photos)
            .FirstOrDefaultAsync(x => x.NormalizedUserName == request.username.ToUpperInvariant());

        if (user == null || user.UserName == null)
        {
            return Unauthorized("Invalid username or password");
        }

        var result = await userManager.CheckPasswordAsync(user, request.password);

        if (!result)
        {
            return Unauthorized("Invalid username or password");
        }

        return new UserResponse
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = await tokenService.CreateToken(user),
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url
        };
    }

    private async Task<bool> UserExistsAsync(string username) =>
        await userManager.Users.AnyAsync(u => u.NormalizedUserName == username.ToUpperInvariant());
}