using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Contract;
using WorkOrderApi.Data;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features;

public static class FindWorkOrderById
{
    public class Query : IRequest<Result<FindWorkOrderByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<FindWorkOrderByIdResponse>>
    {
        private readonly WorkOrderContext _context;

        public Handler(WorkOrderContext context)
        {
            _context = context;
        }

        public async Task<Result<FindWorkOrderByIdResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Where(wo => wo.Id == query.Id)
                .Select(wo => new FindWorkOrderByIdResponse
                {
                    Id = wo.Id,
                    EquipmentName = wo.EquipmentName,
                    Description = wo.Description,
                    WorkOrderStatus = wo.WorkOrderStatus,
                    CreatedAt = wo.CreatedAt,
                    Target = wo.Target
                })
                .FirstOrDefaultAsync();
            if (workOrder == null)
            {
                return Result.Failure<FindWorkOrderByIdResponse>(new Error("WorkOrder.NotFound", "Registro não encontrado"));
            }
            return workOrder;
        }
    }
}

public class FindWorkOrderByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("v2/work-orders/{id}", async (int id, ISender sender) =>
        {
            var query = new FindWorkOrderById.Query { Id = id };
            var result = await sender.Send(query);
            if (result.isFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}