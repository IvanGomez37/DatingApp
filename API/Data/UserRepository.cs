namespace API.Data;

using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<IEnumerable<AppUser>> GetAllAsync() 
        => await context.Users
                        .Include(x => x.Photos)
                        .ToListAsync();

    public async Task<AppUser?> GetByIdAsync(int id)
        => await context.Users
                        .Include(x => x.Photos)
                        .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<AppUser?> GetByUsernameAsync(string username) 
        => await context.Users
                        .Include(x => x.Photos)    
                        .SingleOrDefaultAsync(x => x.UserName.ToLower() == username.ToLower());
    public async Task<MemberResponse?> GetMemberAsync(string username) 
        => await context.Users
                        .Where(x => x.UserName.ToLower() == username.ToLower())
                        .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                        .SingleOrDefaultAsync();

    public async Task<IEnumerable<MemberResponse>> GetMembersAsync() 
        => await context.Users
                    .ProjectTo<MemberResponse>(mapper.ConfigurationProvider)
                    .ToListAsync();

    public async Task<bool> SaveAllAsync() 
        => await context.SaveChangesAsync() > 0;

    public void Update(AppUser user) 
        => context.Entry(user).State = EntityState.Modified;
}