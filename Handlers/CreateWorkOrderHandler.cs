using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;
using WorkOrderApi.Data;
using WorkOrderApi.Models;

namespace WorkOrderApi.Handlers;

public class CreateWorkOrderHandler : ICreateWorkOrderHandler
{
    private readonly WorkOrderRepository _repository;

    public CreateWorkOrderHandler(WorkOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateWorkOrderResponse> Handle(CreateWorkOrderRequest command)
    {
        var workOrder = new WorkOrderModel(command.Name, DateTime.Now);
        workOrder.ExecuteWorkOrder();

        await _repository.CreateAsync(workOrder);
        await _repository.Commit();

        return new CreateWorkOrderResponse
        {
            Id = workOrder.Id
        };
    }
}