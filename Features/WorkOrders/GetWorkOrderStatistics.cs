using Carter;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Contracts;
using WorkOrderApi.Data;
using WorkOrderApi.Enums;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.WorkOrders;

public static class GetWorkOrderStatistics {
    public class  Query : IRequest<Result<WorkOrderStatistics>>
    {
   
    }
    
    internal sealed class Handler : IRequestHandler<Query, Result<WorkOrderStatistics>>
    {
        private readonly WorkOrderContext _context;

        public Handler(WorkOrderContext context)
        {
            _context = context;
        }
        
        public async Task<Result<WorkOrderStatistics>> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.WorkOrders
                .AsNoTracking()
                .GroupBy(wo => wo.WorkOrderStatus)
                .Select(wo => new WorkOrderStatistics
                {
                    TotalFinished = wo.Count(o => o.WorkOrderStatus == EWorkOrderStatus.FINISHED),
                    TotalInExecution = wo.Count(o => o.WorkOrderStatus == EWorkOrderStatus.IN_EXECUTION),
                    TotalLate = wo.Count(o => o.WorkOrderStatus == EWorkOrderStatus.LATE),
                })
                .FirstOrDefaultAsync(cancellationToken);
            
            if (result == null)
            {
                return Result.Failure<WorkOrderStatistics>(new Error("WorkOrderStatistics.NotFound", "Não foi encontrada a ordem de serviço"));
            }
            return result;
        }
    }
}

public class GetWorkOrderStatisticsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/work-orders/statistics", async (ISender sender) =>
        {
            var query = new GetWorkOrderStatistics.Query();
            var result = await sender.Send(query);
            if (result.isFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}