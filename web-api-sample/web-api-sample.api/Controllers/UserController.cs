using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using web_api_sample.api.Business;
using web_api_sample.api.Models.Dtos;
using web_api_sample.api.Models.Entities;

namespace web_api_sample.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserBusiness _userBusiness;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, UserBusiness userBusiness, IMapper mapper)
        {
            _logger = logger;
            _userBusiness = userBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria um usuário
        /// </summary>
        /// <param name="user">Objeto do usuário</param>
        /// <response code="201">Usuário criado com sucesso</response>
        /// <response code="400">Ocorreu um erro de validação</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] User user)
        {
            try
            {
                if (await _userBusiness.AddAsync(user))
                {
                    UserDto userDto = _mapper.Map<UserDto>(user);

                    return Ok(userDto);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao salvar o usuário");
                return StatusCode(500, $"Ocorreu um erro ao salvar o usuário. User: {user}");
            }
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <param name="user">Objeto do usuário</param>
        /// <response code="200">Usuário atualizado com sucesso</response>
        /// <response code="400">Ocorreu um erro de validação</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">Nenhum usuário foi encontrado</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            try
            {
                if (id != user.Id || !await _userBusiness.ValidateUsernameAsync(user))
                {
                    return BadRequest();
                }

                if (await _userBusiness.UpdateAsync(user))
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar o usuário");
                return StatusCode(500, $"Ocorreu um erro ao atualizar o usuário. User: {user}, Id: {id}");
            }
        }

        /// <summary>
        /// Deleta um usuário existente
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <response code="200">Usuário deletado com sucesso</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">Nenhum usuário foi encontrado</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _userBusiness.DeleteAsync(id))
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao deletar o usuário");
                return StatusCode(500, $"Ocorreu um erro ao deletar o usuário. Id: {id}");
            }
        }

        /// <summary>
        /// Consulta todos os usuários existentes
        /// </summary>
        /// <response code="200">Usuários consultados com sucesso</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">Nenhum usuário foi encontrado</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<User> users = await _userBusiness.GetAllAsync();

                if (users == null)
                {
                    return NotFound();
                }

                List<UserDto> usersDto = _mapper.Map<List<UserDto>>(users);

                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar o usuário");
                return StatusCode(500, "Ocorreu um erro ao buscar o usuário");
            }
        }

        /// <summary>
        /// Consulta um usuário existente
        /// </summary>
        /// <param name="id">Id do usuário</param>
        /// <response code="200">Usuário consultado com sucesso</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">O usuário requisitado não foi encontrado</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                User user = await _userBusiness.GetByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                UserDto userDto = _mapper.Map<UserDto>(user);

                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar o usuário");
                return StatusCode(500, $"Ocorreu um erro ao buscar o usuário. Id: {id}");
            }
        }
    }
}