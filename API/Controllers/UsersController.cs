using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using System.Security.Claims;
using AutoMapper;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;
    public readonly IMapper _mapper;

    public UsersController(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync()
    {
        var members = await _repository.GetMembersAsync();

        return Ok(members);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberResponse>> GetByUsernameAsync(string username)
    {
        var member = await _repository.GetMemberAsync(username);

        if (member == null) return NotFound();

        return member;
    }

    [HttpPut]
    public async Task<ActionResult> UpdateAsync(MemberUpdateRequest request)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null) return BadRequest("No user found in token");

        var member = await _repository.GetByUsernameAsync(username);
        if (member == null) return NotFound("No user found");

        _mapper.Map(request, member);
        _repository.Update(member);

        if (await _repository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }
}