namespace WorkOrderApi.Contracts;

public class CreateWorkOrderRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Target { get; set; }
}