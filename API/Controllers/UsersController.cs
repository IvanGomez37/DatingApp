using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;

    public UsersController(IUserRepository repository)
    {
        _repository = repository;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync()
    {
        var members = await _repository.GetMembersAsync();

        return Ok(members);
    }

    [HttpGet("{username}")]
public async Task<ActionResult<MemberResponse>>GetByUsernameAsync(string username)
    {
        var member = await _repository.GetMemberAsync(username);
    
        if (member == null) return NotFound();
    
        return member;
    }
}