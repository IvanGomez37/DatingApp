using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using System.Security.Claims;
using AutoMapper;
using API.Interfaces;
using API.Helpers;
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
    public async Task<ActionResult<IEnumerable<MemberResponse>>> GetAllAsync([FromQuery] UserParams userParams)
    {
        var members = await _repository.GetMembersAsync(userParams);

        Response.AddPaginationHeader(members);

        return Ok(members);
    }

    [HttpGet("{username}", Name = "GetByUsername")]
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
        if (user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }
        user.Photos.Add(photo);
        if (await _repository.SaveAllAsync())
        {
            return CreatedAtRoute("GetByUsername", new { username = user.UserName }, _mapper.Map<PhotoResponse>(photo));
            // return _mapper.Map<PhotoResponse>(photo);
        }
        return BadRequest("Failed to add photo");
    }
    [HttpPut("photo/{photoId:int}")]
    public async Task<ActionResult> SetPhotoAsMainAsync(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("No user found");

        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null) return NotFound("No photo found");

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(p => p.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await _repository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to set main photo");
    }

    [HttpDelete("photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _repository.GetByUsernameAsync(User.GetUsername());
        if (user == null) return BadRequest("User not found");
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("This photo can't be deleted");
        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if (await _repository.SaveAllAsync()) return Ok();
        return BadRequest("There was a problem when deleting the photo");
    }
}