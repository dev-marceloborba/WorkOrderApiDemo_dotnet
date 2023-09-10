namespace WorkOrderApi.Handlers;

public interface IDeleteWorkOrderHandler
{
    Task<int> Handle(int id);
}