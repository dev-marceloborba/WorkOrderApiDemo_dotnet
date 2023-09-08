namespace WorkOrderApi.Commands.Responses;

public class FindWorkOrderByIdResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime TargetDate { get; set; }
}