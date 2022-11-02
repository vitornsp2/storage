using findox.Domain.Models.Database;

namespace findox.Domain.Models.Dto;


public class UserDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class UserSessionDto
{
    public string Email { get; set; }
    public string Password { get; set; }

    public UserSessionDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
}

public class UserAllDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public DateTime? CreatedDate { get; set; }
    public virtual ICollection<Document>? Documents { get; set; }
    public virtual ICollection<UserGroup>? UserGroups { get; set; }
    public virtual ICollection<Permission>? Permissions { get; set; }
}