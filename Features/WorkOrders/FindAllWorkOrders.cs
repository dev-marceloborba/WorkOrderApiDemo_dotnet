using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Contracts;
using WorkOrderApi.Data;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.WorkOrders;

public static class FindAllWorkOrders
{
    public class Query : IRequest<Result<IEnumerable<WorkOrderResponse>>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, Result<IEnumerable<WorkOrderResponse>>>
    {
        private readonly WorkOrderContext _context;

        public Handler(WorkOrderContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<WorkOrderResponse>>> Handle(Query query, CancellationToken cancellationToken)
        {
            try
            {
                var workOrders = await _context.WorkOrders
                    .AsNoTracking()
                    .Select(wo => new WorkOrderResponse
                    {
                        Id = wo.Id,
                        EquipmentName = wo.EquipmentName,
                        Description = wo.Description,
                        CreatedAt = wo.CreatedAt,
                        Target = wo.Target,
                        WorkOrderStatus = wo.WorkOrderStatus
                    })
                .ToListAsync();

                return workOrders;
            }
            catch
            {
                return Result.Failure<IEnumerable<WorkOrderResponse>>(new Error("FindWorkOrders.Query", "Erro ao buscar as ordens de serviço"));
            }
        }
    }
}

public class FindAllWorkOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/work-orders", async (ISender sender) =>
        {
            var query = new FindAllWorkOrders.Query();
            var result = await sender.Send(query);
            if (result.isFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}