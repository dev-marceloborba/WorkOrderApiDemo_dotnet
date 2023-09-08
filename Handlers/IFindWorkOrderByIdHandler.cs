using WorkOrderApi.Commands.Requests;
using WorkOrderApi.Commands.Responses;

namespace WorkOrderApi.Handlers;

public interface IFindWorkOrderByIdHandler
{
    Task<FindWorkOrderByIdResponse> Handle(FindWorkOrderByIdRequest command);
}