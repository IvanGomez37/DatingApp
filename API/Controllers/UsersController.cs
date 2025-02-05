using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using System.Security.Claims;
using AutoMapper;
using API.Interfaces;
using API.Extensions;
using API.Entities;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository repository, IMapper mapper, IPhotoService photoService)
    {
        _repository = repository;
        _mapper = mapper;
        _photoService = photoService;
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

        var member = await _repository.GetByUsernameAsync(User.GetUsername());
        if (member == null) return NotFound("No user found");

        _mapper.Map(request, member);
        _repository.Update(member);

        if (await _repository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update user");
    }

    [HttpPost("photo")]
    public async Task<ActionResult<PhotoResponse>> AddPhotoAsync(IFormFile file)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("No user found");

        var result = await _photoService.AddPhotoAsync(file);
        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        user.Photos.Add(photo);
        if (await _repository.SaveAllAsync())
        {
            return _mapper.Map<PhotoResponse>(photo);
        }
        return BadRequest("Failed to add photo");
    }
}