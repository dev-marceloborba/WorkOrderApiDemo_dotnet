using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WorkOrderApi.Contracts;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.Authentication;

public static class AuthenticateUser {
    public class Command: IRequest<Result<AuthenticationResponse>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    
    public class Validator: AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(au => au.UserName).NotEmpty();
            RuleFor(au => au.Password).NotEmpty().MaximumLength(6);
        }
    }
    
    public static class AuthenticationService
    {
        public static string GenerateToken(string email)
        {
            return email;
        }
    }
    
    internal sealed class Handler : IRequestHandler<Command, Result<AuthenticationResponse>>
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IValidator<Command> _validator;

        public Handler(UserManager<IdentityUser> userManager, IValidator<Command> validator)
        {
            _userManager = userManager;
            _validator = validator;
        }

        public async Task<Result<AuthenticationResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user is null)
            {
                return Result.Failure<AuthenticationResponse>(new Error("AuthenticateUser.NotFound", "Usuário não existe"));
            }
            var token = AuthenticationService.GenerateToken(user.Email);
            var result = new AuthenticationResponse { Token = token, Expiration = DateTime.Now};
            return result;
        }
    }
}

public class AuthenticateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/work-orders/login", async (AuthenticationRequest request, ISender sender) =>
        {
            var command = new AuthenticateUser.Command { UserName = request.UserName, Password = request.Password};
            var result = await sender.Send(command);
            if (result.isFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}