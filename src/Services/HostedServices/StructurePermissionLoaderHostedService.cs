using Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Services.HostedServices;

/// <summary>
/// Mikroservis ishga tushganda fonda AuthApi ga murojaat qilib,
/// barcha tegishli struktura va ruxsatlarni yuklab oluvchi Hosted Service.
/// Agar AuthApi serveri ayni vaqtda tayyor bo'lmasa, eksponentsial qayta urinish (Retry) qiladi.
/// </summary>
public class StructurePermissionLoaderHostedService(
    IServiceScopeFactory scopeFactory,
    ILogger<StructurePermissionLoaderHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Ilova to'liq ishga tushib olishi uchun 1 soniya kutamiz
        await Task.Delay(1000, stoppingToken);

        const int maxAttempts = 6;
        for (int attempt = 1; attempt <= maxAttempts && !stoppingToken.IsCancellationRequested; attempt++)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var loader = scope.ServiceProvider.GetRequiredService<IStructurePermissionLoaderService>();
                var count = await loader.LoadStructurePermissionsAsync(stoppingToken);

                if (count >= 0)
                {
                    logger.LogInformation("Strukturalar ruxsatlari ({Attempt}-urinishda) muvaffaqiyatli yuklandi. Struktur soni: {Count}", attempt, count);
                    break;
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Strukturalar ruxsatini yuklashda xatolik yuz berdi ({Attempt}/{MaxAttempts}). Qayta urinilmoqda...", attempt, maxAttempts);
            }

            if (attempt < maxAttempts)
            {
                var delaySeconds = Math.Min(30, Math.Pow(2, attempt));
                await Task.Delay(TimeSpan.FromSeconds(delaySeconds), stoppingToken);
            }
        }
    }
}
