using FCG_Payments.Application.Service;
using FCG_Payments.Domain.Interface.Repository;
using FCG_Payments.Domain.Interface.Service;
using FCG_Payments.Infrastructure.Data;
using FCG_Payments.Infrastructure.Repository;
using FCG_Payments.WebAPI.Extension;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serviços customizados
builder.AddSwagger();
builder.AddJwtAuthentication();
builder.AddDbContext();

// Serviços padrão ASP.NET
builder.Services.AddAuthorization();
builder.Services.AddControllers()
                .AddNewtonsoftJson();

// Injeção de dependência
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var app = builder.Build();

// Aplicar migrações pendentes ao iniciar a aplicações
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwagger();

// Redirecionamento da raiz para /swagger
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();