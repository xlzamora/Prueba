using System.Data;
using ClienteApi.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ClienteApi.Data;

public class ClienteRepository : IClienteRepository
{
    private readonly string _connectionString;

    public ClienteRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' no configurada.");
    }

    private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

    public async Task<IEnumerable<ClienteReadDto>> GetAllAsync()
    {
        using var connection = CreateConnection();
        return await connection.QueryAsync<ClienteReadDto>(
            "dbo.Cliente_GetAll",
            commandType: CommandType.StoredProcedure);
    }

    public async Task<ClienteReadDto?> GetByIdAsync(int clienteId)
    {
        using var connection = CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<ClienteReadDto>(
            "dbo.Cliente_GetById",
            new { clienteId },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> CreateAsync(ClienteCreateDto dto)
    {
        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "dbo.Cliente_Insert",
            new
            {
                nombre = dto.Nombre,
                edad = dto.Edad,
                fechaNacimiento = dto.FechaNacimiento.Date,
                salario = dto.Salario
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<bool> UpdateAsync(int clienteId, ClienteUpdateDto dto)
    {
        using var connection = CreateConnection();
        var affectedRows = await connection.ExecuteScalarAsync<int>(
            "dbo.Cliente_Update",
            new
            {
                clienteId,
                nombre = dto.Nombre,
                edad = dto.Edad,
                fechaNacimiento = dto.FechaNacimiento.Date,
                salario = dto.Salario
            },
            commandType: CommandType.StoredProcedure);

        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int clienteId)
    {
        using var connection = CreateConnection();
        var affectedRows = await connection.ExecuteScalarAsync<int>(
            "dbo.Cliente_Delete",
            new { clienteId },
            commandType: CommandType.StoredProcedure);

        return affectedRows > 0;
    }
}
