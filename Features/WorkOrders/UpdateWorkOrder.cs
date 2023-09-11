using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkOrderApi.Contracts;
using WorkOrderApi.Data;
using WorkOrderApi.Enums;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features;

public static class UpdateWorkOrder
{
    public class Command : IRequest<Result<WorkOrderResponse>>
    {
        public int Id { get; set; }
        public string? EquipmentName { get; set; }
        public string? Description { get; set; }
        public EWorkOrderStatus WorkOrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Target { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public void UpdateWorkOrder()
        {
            RuleFor(c => c.EquipmentName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(c => c.Description).NotEmpty().MaximumLength(200);
            RuleFor(c => c.Target).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<WorkOrderResponse>>
    {
        private readonly WorkOrderContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(WorkOrderContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<WorkOrderResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<WorkOrderResponse>(new Error("UpdateWorkOrder.Validation", validationResult.ToString()));
            }

            var workOrder = await _context.WorkOrders.Where(wo => wo.Id == request.Id).FirstOrDefaultAsync();
            if (workOrder == null)
            {
                return Result.Failure<WorkOrderResponse>(new Error("UpdateWorkOrder.NotFound", "Ordem de serviço não encontrada"));
            }

            request.Adapt(workOrder);

            _context.WorkOrders.Update(workOrder);
            await _context.SaveChangesAsync(cancellationToken);

            return workOrder.Adapt<WorkOrderResponse>();
        }
    }
}

public class UpdateWorkOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("v2/work-orders/{id}", async (int id, UpdateWorkOrderRequest request, ISender sender) =>
        {
            request.Id = id;
            var command = request.Adapt<UpdateWorkOrder.Command>();
            var result = await sender.Send(command);

            if (result.isFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}