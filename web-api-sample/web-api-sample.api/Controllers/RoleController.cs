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
    public class RoleController : ControllerBase
    {
        private readonly ILogger<RoleController> _logger;
        private readonly RoleBusiness _roleBusiness;
        private readonly IMapper _mapper;

        public RoleController(ILogger<RoleController> logger, RoleBusiness roleBusiness, IMapper mapper)
        {
            _logger = logger;
            _roleBusiness = roleBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Cria uma role
        /// </summary>
        /// <param name="role">Objeto da role</param>
        /// <response code="201">Role criada com sucesso</response>
        /// <response code="400">Ocorreu um erro de validação</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Post([FromBody] Role role)
        {
            try
            {
                if (await _roleBusiness.AddAsync(role))
                {
                    RoleDto roleDto = _mapper.Map<RoleDto>(role);

                    return CreatedAtAction(nameof(GetById), new { id = role.Id }, roleDto);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao salvar a role");
                return StatusCode(500, $"Ocorreu um erro ao salvar a role. Role: {role}");
            }
        }

        /// <summary>
        /// Atualiza uma role existente
        /// </summary>
        /// <param name="id">Id da role</param>
        /// <param name="role">Objeto da role</param>
        /// <response code="200">Role atualizada com sucesso</response>
        /// <response code="400">Ocorreu um erro de validação</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">Nenhuma role foi encontrada</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put(int id, [FromBody] Role role)
        {
            try
            {
                if (id != role.Id || !await _roleBusiness.ValidateRoleNameAsync(role))
                {
                    return BadRequest();
                }

                if (await _roleBusiness.UpdateAsync(role))
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao atualizar a role");
                return StatusCode(500, $"Ocorreu um erro ao atualizar a role. Role: {role}, Id: {id}");
            }
        }

        /// <summary>
        /// Deleta uma role existente
        /// </summary>
        /// <param name="id">Id da role</param>
        /// <response code="200">Role deletada com sucesso</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">Nenhuma role foi encontrada</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (await _roleBusiness.DeleteAsync(id))
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
        /// Consulta todas as roles existentes
        /// </summary>
        /// <response code="200">Roles consultadas com sucesso</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">Nenhuma role foi encontrada</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                List<Role> roles = await _roleBusiness.GetAllAsync();

                if (roles == null)
                {
                    return NotFound();
                }

                List<RoleDto> rolesDto = _mapper.Map<List<RoleDto>>(roles);

                return Ok(rolesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar a role");
                return StatusCode(500, "Ocorreu um erro ao buscar a role");
            }
        }

        /// <summary>
        /// Consulta uma role existente
        /// </summary>
        /// <param name="id">Id da role</param>
        /// <response code="200">Role consultada com sucesso</response>
        /// <response code="401">É necessário se autenticar para completar a requisição</response>
        /// <response code="404">A role requisitada não foi encontrada</response>
        /// <response code="500">Ocorreu uma exceção ao processar sua requisição</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetById(int id)
        {
            try
            {
                Role role = await _roleBusiness.GetByIdAsync(id);

                if (role == null)
                {
                    return NotFound();
                }

                RoleDto roleDto = _mapper.Map<RoleDto>(role);

                return Ok(roleDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao buscar a role");
                return StatusCode(500, $"Ocorreu um erro ao buscar a role. Id: {id}");
            }
        }
    }
}