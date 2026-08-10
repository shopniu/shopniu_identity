using Shopniu_identity.Infrastructure.Configuration;
using Shopniu_identity.Infrastructure.Configuration.Authentication;
using Shopniu_identity.Infrastructure.Configuration.Pipeline;


var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddApiServices()
.AddAuthenticationServices()
.AddInfrastructureServices(builder.Configuration)
.AddPersistenceServices(builder.Configuration)
.AddApplicationServices();

var app = builder.Build();
await app.UseApiPipeline();


app.Run();