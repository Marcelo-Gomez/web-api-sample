using web_api_sample.api.Models.Entities;

namespace web_api_sample.api.Authentication
{
    public interface ITokenFactory
    {
        string GenerateToken(User user);
    }
}