using ClienteApi.Models;

namespace ClienteApi.Data;

public interface IClienteRepository
{
    Task<IEnumerable<ClienteReadDto>> GetAllAsync();
    Task<ClienteReadDto?> GetByIdAsync(int clienteId);
    Task<int> CreateAsync(ClienteCreateDto dto);
    Task<bool> UpdateAsync(int clienteId, ClienteUpdateDto dto);
    Task<bool> DeleteAsync(int clienteId);
}
