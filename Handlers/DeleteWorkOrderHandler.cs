
using WorkOrderApi.Data;
using WorkOrderApi.Models;

namespace WorkOrderApi.Handlers;

public class DeleteWorkOrderHandler : IDeleteWorkOrderHandler
{
    private readonly WorkOrderRepository _repository;

    public DeleteWorkOrderHandler(WorkOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(int id)
    {
        var workOrder = new WorkOrder { Id = id };

        _repository.Delete(workOrder);

        await _repository.Commit();

        return workOrder.Id;
    }
}