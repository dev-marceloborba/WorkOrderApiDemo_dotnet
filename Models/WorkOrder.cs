using WorkOrderApi.Enums;

namespace WorkOrderApi.Models;

public class WorkOrder
{
    public int Id { get; set; }
    public string? EquipmentName { get; set; }
    public string? Description { get; set; }
    public EWorkOrderStatus WorkOrderStatus { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime Target { get; set; }

    public WorkOrder()
    {

    }

    public void CheckIfItsLate(DateTime currentDate)
    {
        if (currentDate > Target)
            WorkOrderStatus = EWorkOrderStatus.LATE;
    }

    public void ExecuteWorkOrder() => WorkOrderStatus = EWorkOrderStatus.IN_EXECUTION;

    public void FinishWorkOrder() => WorkOrderStatus = EWorkOrderStatus.FINISHED;

}