using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Filters;

public class ApiKeyValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var apiKey = value as string;

        if (string.IsNullOrEmpty(apiKey))
        {
            return new ValidationResult("API-nyckeln är obligatorisk.");
        }

        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var expectedApiKey = config.GetValue<string>("ApiKey");

        if (!apiKey.Equals(expectedApiKey))
        {
            return new ValidationResult("Ogiltig API-nyckel.");
        }

        return ValidationResult.Success!;
    }
}