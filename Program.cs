using Microsoft.EntityFrameworkCore;
using FrutosDeGoias.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necessário para o Swagger clássico
builder.Services.AddSwaggerGen();           // Habilita o gerador da UI

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Ativa o endpoint do Swagger
    app.UseSwaggerUI(); // Ativa a interface visual web em /swagger
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("PermitirFrontend");
app.MapControllers();
app.Run();