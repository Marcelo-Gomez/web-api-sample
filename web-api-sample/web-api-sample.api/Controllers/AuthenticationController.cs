using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using web_api_sample.api.Authentication;
using web_api_sample.api.Business;
using web_api_sample.api.Models.Entities;

namespace web_api_sample.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly UserBusiness _userBusiness;
        private readonly ITokenFactory _tokenFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthenticationController(ILogger<AuthenticationController> logger, UserBusiness userBusiness, ITokenFactory tokenFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _logger = logger;
            _userBusiness = userBusiness;
            _tokenFactory = tokenFactory;
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Cria o Token
        /// </summary>
        /// <param name="username">Nome do usuário</param>
        /// <param name="password">Senha do usuário</param>
        /// <response code="200">Token criado com sucesso</response>
        /// <response code="401">Usuário ou senha estão incorretos</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login(string username, string password)
        {
            try
            {
                User user = await _userBusiness.LoginAsync(username, password);

                if (user == null)
                {
                    return Unauthorized(new
                    {
                        message = "Usuário ou senha estão incorretos"
                    });
                }

                return Ok(new
                {
                    token = _tokenFactory.GenerateToken(user),
                    expireInSeconds = Convert.ToInt32(_jwtOptions.ValidFor.TotalSeconds)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao efetuar o login");
                return StatusCode(500, $"Ocorreu um erro ao efetuar o login. Username: {username}");
            }
        }
    }
}