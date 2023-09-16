namespace WorkOrderApi.Contracts;

public class WorkOrderStatistics {
    public int TotalInExecution { get; set; }
    public int TotalFinished { get; set; }
    public int TotalLate { get; set; }
}