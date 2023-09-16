using WorkOrderApi.Enums;

namespace WorkOrderApi.Contracts;

public class UpdateWorkOrderRequest
{
    public int Id { get; set; }
    public string EquipmentName { get; set; }
    public string Description { get; set; }
    public EWorkOrderStatus WorkOrderStatus { get; set; }
    public DateTime Target { get; set; }
}