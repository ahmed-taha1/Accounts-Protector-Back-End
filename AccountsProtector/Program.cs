using AccountsProtector.Core.Domain.Entities;
using AccountsProtector.Core.ServiceContracts;
using AccountsProtector.Core.Services;
using DataAccessLayer.UnitOfWork;
using AccountsProtector.Infrastructure.AppDbContext;
using AccountsProtector.Infrastructure.UnitOfWork;
using AccountsProtector.UI.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// database connection
builder.Services.AddDbContext<AppDbContext>
    (options => options.UseSqlServer
        (builder.Configuration.GetConnectionString
            ("myConnectionString")));

// Identity
builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<User, IdentityRole<Guid>, AppDbContext, Guid>>()
    .AddRoleStore<RoleStore<IdentityRole<Guid>, AppDbContext, Guid>>();

// data access
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// services
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IJwtService, JwtService>();



builder.Services.AddAuthorization(op => {});

// authorization filter
builder.Services.AddControllers(op =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    op.Filters.Add(new AuthorizeFilter(policy));
}).AddXmlSerializerFormatters();

// Add Jwt Auth
builder.Services.AddCustomJwtAuth(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

// Add Swagger with custom configuration to support authorization
builder.Services.AddCustomSwaggerConfiguration();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
