namespace WorkOrderApi.Commands.Requests;

public record CreateWorkOrderRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Target { get; set; }
}