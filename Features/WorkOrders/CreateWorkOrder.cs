using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using WorkOrderApi.Contracts;
using WorkOrderApi.Data;
using WorkOrderApi.Models;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.WorkOrders;

public static class CreateWorkOrder
{
    public class Command : IRequest<Result<int>>
    {
        public string EquipmentName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Target { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public void CreateWorkOrder()
        {
            RuleFor(c => c.EquipmentName).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(c => c.Description).NotEmpty().MaximumLength(200);
            RuleFor(c => c.Target).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<int>>
    {
        private readonly WorkOrderContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(WorkOrderContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<int>(new Error(
                    "CreateWorkOrder.Validation",
                    validationResult.ToString()
                ));
            }

            var workOrder = new WorkOrder
            {
                EquipmentName = request.EquipmentName,
                Description = request.Description,
                Target = request.Target
            };

            _context.Add(workOrder);

            await _context.SaveChangesAsync(cancellationToken);

            return workOrder.Id;
        }
    }
}


public class CreateWorkOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/work-orders", async (CreateWorkOrderRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateWorkOrder.Command>();
            var result = await sender.Send(command);
            if (result.isFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}