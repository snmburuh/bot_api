using Carter;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Bot.Application.Behaviors;
using Bot.Application.Contracts;
using Bot.Application.HttpHandler;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Bot.Api",
        Description = "bot Bot Api",
        TermsOfService = new Uri("https://co-operativebank.co.ke/terms"),
        Contact = new OpenApiContact
        {
            Name = "Samwel Mburu",
            Email = "smburu@co-operativebank.co.ke",
            Url = new Uri("https://co-operativebank.co.ke"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under OpenApiLicense",
            Url = new Uri("https://co-operativebank.co.ke/license"),
        }
    });
});

var configuration = new ConfigurationBuilder()
                               .AddJsonFile("appsettings.json")
                               .AddEnvironmentVariables()
                               .Build();

        builder.Services.AddCarter();
        builder.Services.Configure<AppSettings>(
        builder.Configuration.GetSection("Settings"));
        builder.Services.AddSingleton(sp =>
        sp.GetRequiredService<IOptions<AppSettings>>().Value);
builder.Services.AddMediatR(Bot.Application.ApplicationAssembly.Instance);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddValidatorsFromAssembly(
    Bot.Application.ApplicationAssembly.Instance,
    includeInternalTypes: true);
builder.Services.AddHttpClient();
        builder.Services.AddScoped<IHttpClientFactoryService, HttpClientFactoryService>();
// Configure Redis options
builder.Services.AddStackExchangeRedisCache(options =>
{
    string connection = builder.Configuration.GetValue<string>("Redis");

    options.Configuration = connection;
});

var app = builder.Build();
app.MapCarter();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/bot", () =>
{
    return "Your friendly AI powered Bot!";
})
.WithName("Slogan")
.WithOpenApi();

app.Run();

