using Asp.Versioning;
using Asp.Versioning.Builder;
using Carter;
using DentalSys.Api.Behaviors;
using DentalSys.Api.Database;
using DentalSys.Api.Features.Identity;
using DentalSys.Api.Features.Patients;
using DentalSys.Api.Middleware;
using DentalSys.Api.OpenApi;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => 
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
//builder.Services.AddDbContext<DentalDbContext>(options => options.UseInMemoryDatabase("InMemoDb")); //Not supported transaction
builder.Services.AddDbContext<DentalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DentalSysDb")));


builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddCustomAuthorizationPolicies();

//builder.Services.AddMemoryCache(); // Use for memory cache service
builder.Services.AddStackExchangeRedisCache(options =>
{
    var connection = builder.Configuration.GetConnectionString("RedisConnection");
    if (string.IsNullOrEmpty(connection))
    {
        throw new ArgumentException("Redis connection string is not configured.");
    }
    options.Configuration = connection;
    options.InstanceName = "SampleInstance:";
});


builder.Services.AddMediatR(config =>
{
    //config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.RegisterServicesFromAssemblyContaining<Program>();
    config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
    config.AddOpenBehavior(typeof(CachingPipelineBehavior<,>));
    config.AddOpenBehavior(typeof(TransactionPipelineBehavior<,>));


});

//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>));
//builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingPipelineBehavior<,>));



builder.Services.AddPatientFeatures();
builder.Services.AddIdentityFeatures();




builder.Services.AddValidatorsFromAssemblyContaining<Program>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCarter();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
        options.AssumeDefaultVersionWhenUnspecified = true;
    });


builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .HasApiVersion(new ApiVersion(3))
    .Build();

RouteGroupBuilder versionGroup = app.MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

versionGroup.MapCarter();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

