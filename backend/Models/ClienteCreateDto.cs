using System.ComponentModel.DataAnnotations;

namespace ClienteApi.Models;

public class ClienteCreateDto
{
    [Required]
    [StringLength(120, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 120)]
    public int Edad { get; set; }

    [DataType(DataType.Date)]
    [NoFutureDate]
    public DateTime FechaNacimiento { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Salario { get; set; }
}
