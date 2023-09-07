using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;

namespace WorkOrderApi.Handlers;

public interface ICreateWorkOrderHandler
{
    Task<CreateWorkOrderResponse> Handle(CreateWorkOrderRequest command);
}