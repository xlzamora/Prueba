using ClienteApi.Data;
using ClienteApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClienteApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteRepository _repository;
    private readonly ILogger<ClientesController> _logger;

    public ClientesController(IClienteRepository repository, ILogger<ClientesController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>Obtiene todos los clientes.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ClienteReadDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var clientes = await _repository.GetAllAsync();
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar clientes.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse { Message = "Error interno al listar clientes.", Details = ex.Message });
        }
    }

    /// <summary>Obtiene un cliente por ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var cliente = await _repository.GetByIdAsync(id);
            if (cliente is null)
            {
                return NotFound(new ApiErrorResponse { Message = $"Cliente con id {id} no encontrado." });
            }

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener cliente {ClienteId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse { Message = "Error interno al obtener cliente.", Details = ex.Message });
        }
    }

    /// <summary>Crea un nuevo cliente.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ClienteReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDto dto)
    {
        try
        {
            var newId = await _repository.CreateAsync(dto);
            var created = await _repository.GetByIdAsync(newId);

            return CreatedAtAction(nameof(GetById), new { id = newId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente.");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse { Message = "Error interno al crear cliente.", Details = ex.Message });
        }
    }

    /// <summary>Actualiza un cliente existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDto dto)
    {
        try
        {
            var updated = await _repository.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound(new ApiErrorResponse { Message = $"Cliente con id {id} no encontrado." });
            }

            var cliente = await _repository.GetByIdAsync(id);
            return Ok(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar cliente {ClienteId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse { Message = "Error interno al actualizar cliente.", Details = ex.Message });
        }
    }

    /// <summary>Elimina un cliente por ID.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _repository.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new ApiErrorResponse { Message = $"Cliente con id {id} no encontrado." });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente {ClienteId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse { Message = "Error interno al eliminar cliente.", Details = ex.Message });
        }
    }
}
