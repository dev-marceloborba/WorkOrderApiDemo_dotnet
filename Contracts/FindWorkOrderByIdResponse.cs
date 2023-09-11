using WorkOrderApi.Enums;

namespace WorkOrderApi.Contract;

public class FindWorkOrderByIdResponse
{
    public int Id { get; set; }
    public string? EquipmentName { get; set; }
    public string? Description { get; set; }
    public EWorkOrderStatus WorkOrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime Target { get; set; }
}