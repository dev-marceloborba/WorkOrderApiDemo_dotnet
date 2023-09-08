using AutoMapper;
using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;
using WorkOrderApi.Data;
using WorkOrderApi.Models;

namespace WorkOrderApi.Handlers;

public class CreateWorkOrderHandler : ICreateWorkOrderHandler
{
    private readonly WorkOrderRepository _repository;
    private readonly IMapper _mapper;

    public CreateWorkOrderHandler(WorkOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CreateWorkOrderResponse> Handle(CreateWorkOrderRequest command)
    {
        var workOrder = _mapper.Map<WorkOrder>(command);
        workOrder.ExecuteWorkOrder();

        await _repository.CreateAsync(workOrder);
        await _repository.Commit();

        return new CreateWorkOrderResponse
        {
            Id = workOrder.Id
        };
    }
}