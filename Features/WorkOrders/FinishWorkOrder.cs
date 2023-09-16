using Carter;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Contracts;
using WorkOrderApi.Data;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.WorkOrders;

public static class FinishWorkOrder {
    public class Query : IRequest<Result<WorkOrderResponse>>
    {
        public int Id { get; set; }
    }
    
    internal sealed class Handler : IRequestHandler<Query, Result<WorkOrderResponse>>
    {
        private readonly WorkOrderContext _context;

        public Handler(WorkOrderContext context)
        {
            _context = context;
        }

        public async Task<Result<WorkOrderResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var workOrder = await _context.WorkOrders
                .Where(wo  => wo.Id == query.Id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (workOrder == null)
            {
                return Result.Failure<WorkOrderResponse>(new Error("FinishWorkOrder.NotFound", "Ordem de serviço não localizada"));
            }
            
            workOrder.FinishWorkOrder();
            
            await _context.SaveChangesAsync(cancellationToken);
            
            var result = workOrder.Adapt<WorkOrderResponse>();
            
            return result;
        }
    }
}

public class FinishWorkOrderEnpoint : ICarterModule 
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/work-orders/finish/{id}", async (int id, ISender sender ) =>
        {
            var query = new FinishWorkOrder.Query { Id = id};
            var result = await sender.Send(query);
            if (result.isFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}