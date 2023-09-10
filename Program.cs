using Carter;
using FluentValidation;
using WorkOrderApi.Data;
using WorkOrderApi.Features.WorkOders;
using WorkOrderApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var assembly = typeof(Program).Assembly;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
});
builder.Services.AddCarter();
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddDbContext<WorkOrderContext>();
builder.Services.AddTransient<WorkOrderRepository>();
builder.Services.AddTransient<ICreateWorkOrderHandler, CreateWorkOrderHandler>();
builder.Services.AddTransient<IUpdateWorkOrderHandler, UpdateWorkOrderHandler>();
builder.Services.AddTransient<IDeleteWorkOrderHandler, DeleteWorkOrderHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CreateWorkOrder.MapEndpoint(app);

app.UseHttpsRedirection();

app.UseCors();

app.MapCarter();

app.UseAuthorization();

app.MapControllers();

app.Run();
