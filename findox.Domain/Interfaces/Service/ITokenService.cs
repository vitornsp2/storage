
namespace findox.Domain.Interfaces.Service
{
    public interface ITokenService
    {
        string Encode(long id, string role);
    }
}