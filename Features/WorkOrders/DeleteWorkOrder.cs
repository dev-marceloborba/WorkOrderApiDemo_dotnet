using Carter;
using MediatR;
using WorkOrderApi.Data;
using WorkOrderApi.Models;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features;

public static class DeleteWorkOrder
{
    public class Command : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<int>>
    {
        private readonly WorkOrderContext _context;

        public Handler(WorkOrderContext context)
        {
            _context = context;
        }

        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            var workOrder = new WorkOrder { Id = request.Id };
            try
            {
                _context.WorkOrders.Remove(workOrder);
                await _context.SaveChangesAsync(cancellationToken);
                return workOrder.Id;
            }
            catch
            {
                return Result.Failure<int>(new Error("DeleteWorkOrder.Delete", "Erro ao excluir ordem de serviço"));
            }
        }
    }
}

public class DeleteWorkOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("v1/work-orders/{id}", async (int id, ISender sender) =>
        {
            var command = new DeleteWorkOrder.Command { Id = id };
            var result = await sender.Send(command);
            if (result.isFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}