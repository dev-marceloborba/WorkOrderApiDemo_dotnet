namespace WorkOrderApi.Contracts;

public class CreateWorkOrderRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Target { get; set; }
}