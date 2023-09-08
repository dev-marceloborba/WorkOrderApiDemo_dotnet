using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;

namespace WorkOrderApi.Handlers;

public interface IUpdateWorkOrderHandler
{
    Task<UpdateWorkOrderResponse> Handle(UpdateWorkOrderRequest command);
}