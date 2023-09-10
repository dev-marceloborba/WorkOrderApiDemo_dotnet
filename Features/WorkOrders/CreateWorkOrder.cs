using Carter;
using FluentValidation;
using MediatR;
using WorkOrderApi.Data;
using WorkOrderApi.Models;

namespace WorkOrderApi.Features.WorkOders;

public static class CreateWorkOrder
{
    public class Command : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Target { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public void CreateWorkOrder()
        {
            RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
            RuleFor(c => c.Description).NotEmpty().MaximumLength(200);
            RuleFor(c => c.Target).NotEmpty();
        }
    }

    internal sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly WorkOrderContext _context;
        private readonly IValidator<Command> _validator;

        public Handler(WorkOrderContext context, IValidator<Command> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            var workOrder = new WorkOrder
            {
                EquipmentName = request.Name,
                Description = request.Description,
                Target = request.Target
            };

            _context.Add(workOrder);

            await _context.SaveChangesAsync(cancellationToken);

            return workOrder.Id;
        }
    }

    public class CreateWorkOrderEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("v2/work-orders", async (Command command, ISender sender) =>
            {
                var workOrderId = await sender.Send(command);
                return Results.Ok(workOrderId);
            });
        }
    }
}