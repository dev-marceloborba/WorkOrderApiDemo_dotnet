using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Carter;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
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
        public static string GenerateToken(string name, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Settings.Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                  new Claim(ClaimTypes.Name, name),
                  new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
            var token = AuthenticationService.GenerateToken(user.UserName, user.Email);
            var result = new AuthenticationResponse { Token = token, Expiration = DateTime.UtcNow.AddHours(2)};
            return result;
        }
    }
}

public class AuthenticateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/login", async (AuthenticationRequest request, ISender sender) =>
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