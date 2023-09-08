namespace WorkOrderApi.Commands.Requests;

public class UpdateWorkOrderRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Target { get; set; }
}