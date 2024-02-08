using DataAccessLayer.UnitOfWork;
using DataLayer.DataBase;
using DataLayer.Models;
using DataLayer.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServicesLayer.JwtService;
using ServicesLayer.UserService;

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
