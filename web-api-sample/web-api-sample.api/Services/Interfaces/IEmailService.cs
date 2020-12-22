using System.Threading.Tasks;
using web_api_sample.api.Models.Dtos;

namespace web_api_sample.api.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(EmailDto email);
    }
}