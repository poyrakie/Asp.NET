using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Entities;

public class ContactEntity
{
    [Key]
    public int Id { get; set; }
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Service { get; set; }
    public string Message { get; set; } = null!;
}
