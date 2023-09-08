using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;
using WorkOrderApi.Data;
using WorkOrderApi.Models;

namespace WorkOrderApi.Handlers;

public class UpdateWorkOrderHandler : IUpdateWorkOrderHandler
{
    private readonly WorkOrderRepository _repository;

    public UpdateWorkOrderHandler(WorkOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<UpdateWorkOrderResponse> Handle(UpdateWorkOrderRequest command)
    {
        var workOrder = await _repository.FindByIdAsync(command.Id);

        //TODO: aplicar mapper.

        _repository.Update(workOrder);
        await _repository.Commit();

        return new UpdateWorkOrderResponse
        {
            Id = workOrder.Id
        };
    }
}