namespace ClienteApi.Models;

public class Cliente
{
    public int ClienteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int Edad { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public decimal Salario { get; set; }
}
