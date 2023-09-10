using System.ComponentModel.DataAnnotations;

namespace WorkOrderApi.Commands.Requests;

public record CreateWorkOrderRequest
{
    [Required]
    [MinLength(2)]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [MaxLength(200)]
    public string Description { get; set; }

    [Required]
    public DateTime Target { get; set; }
}