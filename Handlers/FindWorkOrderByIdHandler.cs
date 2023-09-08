using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;
using WorkOrderApi.Data;

namespace WorkOrderApi.Handlers;

public class FindWorkOrderByIdHandler : IFindWorkOrderByIdHandler
{
    private readonly WorkOrderRepository _repository;

    public FindWorkOrderByIdHandler(WorkOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<FindWorkOrderByIdResponse> Handle(FindWorkOrderByIdRequest command)
    {
        var workOrder = await _repository.FindByIdAsync(command.Id);

        //TODO: usar Mapper.

        return new FindWorkOrderByIdResponse
        {
            Id = workOrder.Id,
            Name = workOrder.EquipmentName,
            CreatedAt = workOrder.CreatedDate,
            TargetDate = workOrder.TargetDate
        };
    }
}