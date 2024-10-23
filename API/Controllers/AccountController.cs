using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> RegisterAsync(RegisterRequest request)
    {   
        if (await UserExistsAsync(request.username))
        {
            return BadRequest("Username already exists");
        }

        return Ok();

        /* using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = request.username,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.password)),
            PasswordSalt = hmac.Key
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        }; */
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> LoginAsync(LoginRequest request)
    {
        var user = await context.Users.SingleOrDefaultAsync(x =>
            x.UserName.ToLower() == request.username.ToLower());
        
        if (user == null)
        {
            return Unauthorized("Invalid username or password");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid username or password");
            }
        }

        return new UserResponse
        {
            Username = user.UserName,
            Token = tokenService.CreateToken(user)
        };

    }

    private async Task<bool> UserExistsAsync(string username) =>
        await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
}