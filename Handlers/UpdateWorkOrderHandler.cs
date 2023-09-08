using AutoMapper;
using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;
using WorkOrderApi.Data;

namespace WorkOrderApi.Handlers;

public class UpdateWorkOrderHandler : IUpdateWorkOrderHandler
{
    private readonly WorkOrderRepository _repository;
    private readonly IMapper _mapper;

    public UpdateWorkOrderHandler(WorkOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UpdateWorkOrderResponse> Handle(UpdateWorkOrderRequest command)
    {
        var workOrder = await _repository.FindByIdAsync(command.Id);

        _mapper.Map(command, workOrder);

        _repository.Update(workOrder);
        await _repository.Commit();

        return new UpdateWorkOrderResponse
        {
            Id = workOrder.Id
        };
    }
}