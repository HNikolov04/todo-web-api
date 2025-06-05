using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Quartz;
using Todo.Application.Abstractions.Services;
using Todo.Application.Abstractions.Services.NotificationService;
using Todo.Domain.Entities;
using Todo.Domain.Repositories;
using Todo.Infrastructure.Authentication;
using Todo.Infrastructure.BackgroundJobs;
using Todo.Infrastructure.Services.NotificationService;
using Todo.Persistence;
using Todo.Persistence.Repositories;
using MediatR;
using Todo.Infrastructure.Idempotence;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// 1. DbContext
services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

services.AddQuartz(configure =>
{
    var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

    configure
        .AddJob<ProcessOutboxMessagesJob>(jobKey)
        .AddTrigger(trigger =>
            trigger.ForJob(jobKey)
                .WithSimpleSchedule(schedule =>
                    schedule.WithIntervalInSeconds(10)
                        .RepeatForever())
        );

    configure.UseMicrosoftDependencyInjectionJobFactory();
});

services.AddQuartzHostedService();

// 2. Identity
builder.Services
    .AddIdentity<User, IdentityRole<Guid>>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 6;

        options.User.RequireUniqueEmail = true;

        options.SignIn.RequireConfirmedEmail = false;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. JWT Configuration
services.Configure<JwtOptions>(config.GetSection("Jwt"));
services.AddScoped<IJwtProvider, JwtProvider>();

services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtOptions = config.GetSection("Jwt").Get<JwtOptions>()!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();

services.AddAuthorization();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Todo.Application.AssemblyReference.Assembly);
});


services.Decorate(
    typeof(INotificationHandler<>),
    typeof(IdempotentDomainEventHandler<>)
);

// 5. Infrastructure
services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<ITodoItemRepository, TodoItemRepository>();
services.AddScoped<IEmailNotificationService, EmailNotificationService>();
services.AddScoped<IPhoneNotificationService, PhoneNotificationService>();

// 6. Controllers
services.AddControllers()
    .AddApplicationPart(typeof(Todo.Presentation.AssemblyReference).Assembly);
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    // Other config...
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

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