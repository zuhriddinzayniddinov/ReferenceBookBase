using Core.Domain.Permissions;
using Core.Infrastructure.Grpc.Extensions;
using Core.Infrastructure.Permissions.Extensions;
using Core.Infrastructure.Permissions.Grpc;
using Core.Infrastructure.Permissions.Services;
using Core.Web.Extensions;
using Domain.Contracts;
using Domain.Enums;
using Infrastructures.Data;
using Infrastructures.Extensions;
using Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

// 1. Core barcha standartlarni yoqish (Configuration, Serilog, DbContext, HybridCache, DistributedLock, Swagger, JWT Auth):
builder.ConfigureDefault<ReferenceBookDbContext>();

// 2. Loyihaning o'z infratuzilma va servislar qatlamini ulash:
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationServices();

// 3. AuthApi bilan gRPC muloqoti sozlamalari:
var authGrpcUrl = builder.Configuration.GetConnectionString("AuthGrpcService")
                  ?? builder.Configuration["AuthGrpcService"]
                  ?? "http://localhost:50051";

// AuthApi gRPC mijozlarini ro'yxatdan o'tkazish
builder.Services.AddCoreGrpcClient<IPermissionSyncGrpcService>(authGrpcUrl);
builder.Services.AddCoreGrpcClient<IStructurePermissionGrpcService>(authGrpcUrl);

// 4. Start vaqtida mikroservis ruxsatlarini (ReferenceBookPermissions) AuthApi ga gRPC orqali avtomatik sinxronizatsiya qilish:
builder.Services.AddCorePermissionAutoSync<ReferenceBookPermissions>(PermissionSyncMode.Grpc);

var app = builder.Build();

// Ma'lumotlar bazasi migratsiyalarini avtomatik qo'llash:
await app.UpMigration<ReferenceBookDbContext>();

app.UseResponseCompression();
app.UseRateLimiter();

// Core standart HTTP pipeline (SecurityHeaders, ExceptionHandling, CorrelationId, Idempotency, Swagger, Auth, SessionValidation):
await app.ConfigureDefault<ReferenceBookDbContext>();

app.MapControllers();

app.Run();
