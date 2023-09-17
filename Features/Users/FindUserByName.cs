using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WorkOrderApi.Contracts;
using WorkOrderApi.Shared;

namespace WorkOrderApi.Features.Users;

public static class FindUserByName {
    public class Query : IRequest<Result<CreaterUserResponse>>
    {
        public string UserName { get; set; }
    }
    
    internal sealed class Handler : IRequestHandler<Query, Result<CreaterUserResponse>>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public Handler(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<CreaterUserResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(query.UserName);
            if (user == null)
            {
                return Result.Failure<CreaterUserResponse>(new Error("FindUserByName.NotFound", "Usuário não existe"));
            }
            return user.Adapt<CreaterUserResponse>();
        }
    }
}

public class FindUserByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/users/{userName}", async (string userName, ISender sender) =>
        {
            var query = new FindUserByName.Query { UserName = userName};
            var result = await sender.Send(query);
            if (result.isFailure)
            {
                return Results.NotFound(result.Error);
            }
            return Results.Ok(result.Value);
        });
    }
}