using ECommerceAPI.Application;
using ECommerceAPI.Application.Features.Commands.Products.UpdateProduct;
using ECommerceAPI.Infrastructure;
using ECommerceAPI.Infrastructure.Filters;
using ECommerceAPI.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Text;
using ECommerceAPI.API.Configurations.CustomColumn;
using Serilog.Core;
using ECommerceAPI.API.Extensions;
using Serilog.Context;
using Microsoft.AspNetCore.Builder;
using ECommerceAPI.API.Filters;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddHttpContextAccessor(); // To access HttpContext in service classes
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureService();

// Add CORS policies
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
          .AllowAnyHeader()
          .AllowAnyMethod()
          .AllowCredentials()));

// Configure Serilog logging
var columnOptions = new ColumnOptions
{
    AdditionalColumns = new Collection<SqlColumn>
    {
        new SqlColumn { ColumnName = "UserName", DataType = System.Data.SqlDbType.NVarChar, PropertyName = "UserName", DataLength = 50, AllowNull = true }
    }
};
columnOptions.Store.Remove(StandardColumn.Properties);
columnOptions.Store.Add(StandardColumn.LogEvent);

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log.txt")
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("MSSQL Server"),
                         tableName: "logs",
                         autoCreateSqlTable: true,
                         columnOptions: columnOptions)
    .Enrich.FromLogContext()
    .Enrich.With<UsernameCustomColumn>()
    .MinimumLevel.Information()
    .CreateLogger();
builder.Host.UseSerilog(log);

// Add HTTP logging
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

// Configure controllers and FluentValidation
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>(); // Validation filter
    options.Filters.Add<RolePermissionFilter>(); // Role Permission filter
})
.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<UpdateProductCommandValidator>())
.ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true);

// Configure Swagger for JWT authentication
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Authentication with multiple JWT schemes
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer("Admin", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
})
.AddJwtBearer("Seller", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
})
.AddJwtBearer("Customer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["Token:Audience"],
        ValidIssuer = builder.Configuration["Token:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null ? expires > DateTime.UtcNow : false,
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role
    };
});

// Add Authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes("Admin");
        policy.RequireRole("Admin");
    });

    options.AddPolicy("SellerPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes("Seller");
        policy.RequireRole("Seller");
    });

    options.AddPolicy("CustomerPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddAuthenticationSchemes("Customer");
        policy.RequireRole("Customer");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect root to Swagger
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

// Global exception handler
app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.UseCors(); // Allow CORS
app.UseHttpsRedirection();

app.UseAuthentication(); // Authenticate users
app.UseAuthorization();  // Authorize users based on policies

app.Use(async (context, next) =>
{
    if (context.User?.Identity?.IsAuthenticated == true)
    {
        // Extract roles from the claims
        var roles = context.User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value);

        // Join roles into a single string
        var rolesString = string.Join(",", roles);

        // Push roles to the log context
        LogContext.PushProperty("UserRoles", rolesString);
    }

    await next();
});


app.MapControllers();

app.Run();
