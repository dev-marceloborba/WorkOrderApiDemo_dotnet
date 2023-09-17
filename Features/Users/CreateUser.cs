using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WorkOrderApi.Contracts;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.Users;

public static class CreateUser {
    public class Command : IRequest<Result<CreaterUserResponse>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
    
    public class Validator: AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(u => u.UserName).NotEmpty();
            RuleFor(u => u.Password).NotEmpty().MinimumLength(6);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
        }
    }
    
    internal sealed class Handler : IRequestHandler<Command, Result<CreaterUserResponse>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IValidator<Command> _validator;

        public Handler(UserManager<IdentityUser> userManager, IValidator<Command> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<Result<CreaterUserResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var userValidation = _validator.Validate(request);
            if (!userValidation.IsValid)
            {
                return Result.Failure<CreaterUserResponse>(new Error("CreateUser.Validation", userValidation.ToString()));
            }
            
            var user = await _userManager.CreateAsync(new IdentityUser() { UserName = request.UserName, Email = request.Email}, request.Password);
            if (!user.Succeeded)
            {
                var getFirstError = (IEnumerable<IdentityError> errors) => errors.First().Description;
                return Result.Failure<CreaterUserResponse>(new Error("CreateUser.UserManager", getFirstError(user.Errors)));
            }
            
            return request.Adapt<CreaterUserResponse>();
        }
    }
}

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/users", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUser.Command>();
            var result = await sender.Send(command);
            if (result.isFailure)
            {
                return Results.BadRequest(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}