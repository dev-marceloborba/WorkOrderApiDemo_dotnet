using WorkOrderApi.Enums;

namespace WorkOrderApi.Models;

public class WorkOrderModel
{
    public int Id { get; private set; }
    public string? EquipmentName { get; private set; }
    public EWorkOrderStatus WorkOrderStatus { get; private set; }
    public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;
    public DateTime TargetDate { get; private set; }

    public WorkOrderModel(string equipmentName, DateTime targetDate)
    {
        EquipmentName = equipmentName;
        TargetDate = targetDate;
    }

    public void CheckIfItsLate(DateTime currentDate)
    {
        if (currentDate > TargetDate)
            WorkOrderStatus = EWorkOrderStatus.LATE;
    }

    public void ExecuteWorkOrder() => WorkOrderStatus = EWorkOrderStatus.IN_EXECUTION;

    public void FinishWorkOrder() => WorkOrderStatus = EWorkOrderStatus.FINISHED;

}