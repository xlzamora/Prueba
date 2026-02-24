using System.ComponentModel.DataAnnotations;

namespace ClienteApi.Models;

public sealed class NoFutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not DateTime dateValue)
        {
            return false;
        }

        return dateValue.Date <= DateTime.UtcNow.Date;
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} no puede ser una fecha futura.";
    }
}
