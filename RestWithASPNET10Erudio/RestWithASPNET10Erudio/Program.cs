// PONTO DE ENTRADA: Configura e inicia a aplicação.
// É onde você registra os serviços (Injeção de Dependência) 
// e define o "pipeline" de execução (Middleware) da API.


using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Identity;
using RestWithASPNET10Erudio.Auth.Contract;
using RestWithASPNET10Erudio.Auth.Contract.Tools;
using RestWithASPNET10Erudio.Configurations;
using RestWithASPNET10Erudio.Files.Importers.Impl;
using RestWithASPNET10Erudio.FIles.Exporters.Factory;
using RestWithASPNET10Erudio.FIles.Exporters.Impl;
using RestWithASPNET10Erudio.FIles.Importers.Factory;
using RestWithASPNET10Erudio.Hypermedia.Filters;
using RestWithASPNET10Erudio.Mail;
using RestWithASPNET10Erudio.Repositories;
using RestWithASPNET10Erudio.Repositories.Impl;         // PersonRepository, BookRepository, GenericRepository
using RestWithASPNET10Erudio.Services;
using RestWithASPNET10Erudio.Services.Impl;  // PersonServicesImpl, BookServicesImpl

var builder = WebApplication.CreateBuilder(args);

builder.AddSerilogLogging();

builder.Services.AddControllers(options =>
{
	options.Filters.Add<HypermediaFilter>();
})
.AddXmlSerializerFormatters();
// .AddContentNegotiacion();

builder.Services.AddDatabaseConfiguration(builder.Configuration);
builder.Services.AddEvolveConfiguration(builder.Configuration, builder.Environment);
builder.Services.AddAuthConfiguration(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddRouteConfig();


builder.Services.AddCorsConfiguration(builder.Configuration);
builder.Services.AddHATEOASConfiguration();

builder.Services.AddEmailConfiguration(builder.Configuration);


builder.Services.AddScoped<IPersonServices, PersonServicesImplV1>();
builder.Services.AddScoped<IBookServices, BookServicesImpl>();
builder.Services.AddScoped<IPersonServicesV2, PersonServicesV2>();

builder.Services.AddScoped<IEmailService, EmailServiceImpl>();
builder.Services.AddScoped<EmailSender>();

builder.Services.AddScoped<CsvImporter>();
builder.Services.AddScoped<XlsxImporter>();
builder.Services.AddScoped<FileImporterFactory>();

builder.Services.AddScoped<CsvExporter>();
builder.Services.AddScoped<XlsxExporter>();
builder.Services.AddScoped<FileExporterFactory>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IFileServices, FileServicesImpl>();

builder.Services.AddScoped<IPersonRepository, PersonRepository>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPasswordHasher, Sha256PasswordHasher>();
builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
builder.Services.AddScoped<IUserAuthService, UserAuthServiceImpl>();
builder.Services.AddScoped<ILoginService, LoginServiceImpl>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseHttpsRedirection();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCorsConfiguration(builder.Configuration);
app.MapControllers();

app.UseHATEOASRoutes();

app.UseSwaggerSpecification();
app.UseScalarConfiguration();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://*:{port}");