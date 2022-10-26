using findox.Domain.Models.Database;

namespace findox.Domain.Models.Dto;

public interface IUserDto
{
    long? Id { get; }
    string? Name { get; }
    string? Password { get; }
    string? Email { get; }
    string? Role { get; }
    DateTime? CreatedDate { get; }
}

public class UserDto : IUserDto
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public interface IUserAllDto
{
    long? Id { get; }
    string? Name { get; }
    string? Password { get; }
    string? Email { get; }
    string? Role { get; }
    DateTime? CreatedDate { get; }
    ICollection<Document>? Documents { get; }
    ICollection<UserGroup>? UserGroups { get; }
    ICollection<Permission>? Permissions { get; }
}

public class UserAllDto : IUserAllDto
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

public interface IUserSessionDto
{
    string Email { get; }
    string Password { get; }
}

public class UserSessionDto : IUserSessionDto
{
    public string Email { get; set; }
    public string Password { get; set; }

    public UserSessionDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
}