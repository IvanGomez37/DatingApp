using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace API.Entities;

[ExcludeFromCodeCoverage]

[Table("Photos")]
public class Photo
{
    public int Id {get; set;}
    public required string Url {get; set;}
    public bool IsMain {get; set;}
    public string? PublicId {get; set;}
    //EF Navigation properties
    public int AppUserId {get; set;}
    public AppUser AppUser {get; set;} = null!;
}