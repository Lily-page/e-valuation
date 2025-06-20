using Microsoft.EntityFrameworkCore;
using Knightfrank.eValuation.WebApi.Data;
using Knightfrank.eValuation.WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "eValuation API", Version = "v1" });
    c.AddSecurityDefinition("AnonymousToken", new()
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-Anonymous-Token",
        Description = "Anonymous token for API access"
    });
});

// Add Entity Framework
builder.Services.AddDbContext<EValuationContext>(options =>
    options.UseInMemoryDatabase("EValuationDb"));

// Add custom services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPropertyService, PropertyService>();

// Add background service for token cleanup
builder.Services.AddHostedService<TokenCleanupService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:12000",
                "https://work-1-txpwxmkwjvwpaixy.prod-runtime.all-hands.dev",
                "https://work-2-txpwxmkwjvwpaixy.prod-runtime.all-hands.dev"
              )
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EValuationContext>();
    context.Database.EnsureCreated();

    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Database initialized successfully");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "eValuation API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root
    });
}

app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

// Add health check endpoint
app.MapGet("/health", () => new { status = "healthy", timestamp = DateTime.UtcNow });

app.Run("http://0.0.0.0:12001");
