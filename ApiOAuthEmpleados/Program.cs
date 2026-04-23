using ApiOAthEmpleados.Data;
using ApiOAthEmpleados.Repositories;
using ApiOAuthEmpleados.Helpers;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAzureClients(factory => 
{
    factory.AddSecretClient(builder.Configuration.GetSection("KeyVault"));
});

/* ESTE OBJETO SOLAMENTE LO NECESITAMOS AQUI, LO DICHO, RECUPERAMOS LOS VALORES
 * Y LOS ASIGNAMOS A UNA CLASE 
 * RECUPERAMOS EL SecretClient PARA LOS SECRETOS DE KEY VAULT */
SecretClient secretClient = builder.Services.BuildServiceProvider().GetService<SecretClient>();
//ACCEDEMOS AL SECRETO
KeyVaultSecret secret = await secretClient.GetSecretAsync("secretsqlazuremqmp");


HelperCifrado.Initialize(builder.Configuration);

// Add services to the container.
HelperActionOAuthService helper = new HelperActionOAuthService(builder.Configuration);
builder.Services.AddSingleton<HelperActionOAuthService>(helper);
builder.Services.AddAuthentication(helper.GetAuthenticationSchema()).AddJwtBearer(helper.GetJWtBearerOptions());
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<HelperEmpleadoToken>();

builder.Services.AddTransient<RepositoryHospital>();
string connection = secret.Value;
builder.Services.AddDbContext<HospitalContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
