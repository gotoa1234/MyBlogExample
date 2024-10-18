using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ServiceDiscovery;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Microsoft.Extensions.Hosting
{
    // 新增常見的 .NET Aspire 服務：服務發現、彈性、運行狀況檢查和 OpenTelemetry。
    // 您的解決方案中的每個服務項目都應引用該項目。
    // 要了解有關使用此項目的更多信息，請參閱 https://aka.ms/dotnet/aspire/service-defaults
    public static class Extensions
    {
        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            builder.ConfigureOpenTelemetry();

            builder.AddDefaultHealthChecks();

            builder.Services.AddServiceDiscovery();

            builder.Services.ConfigureHttpClientDefaults(http =>
            {
                // 預設開啟 彈性處理程序
                http.AddStandardResilienceHandler();

                // 預設開啟 服務發現
                http.AddServiceDiscovery();
            });

            // 取消以下註釋內容可限制"允許"的服務發現方案。
            // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
            // {
            //     options.AllowedSchemes = ["https"];
            // });

            return builder;
        }

        /// <summary>
        /// 配置分散式追蹤和指標收集，允許應用程式的監控和可觀察性。
        /// </summary>        
        public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
        {
            builder.Logging.AddOpenTelemetry(logging =>
            {
                logging.IncludeFormattedMessage = true;
                logging.IncludeScopes = true;
            });

            builder.Services.AddOpenTelemetry()
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation()
                        .AddHttpClientInstrumentation()
                        .AddRuntimeInstrumentation();
                })
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation()
                        // 取消註釋以下行以啟用 gRPC 檢測（需要 OpenTelemetry.Instrumentation.GrpcNetClient 套件）
                        //.AddGrpcClientInstrumentation()
                        .AddHttpClientInstrumentation();
                });

            builder.AddOpenTelemetryExporters();

            return builder;
        }

        private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
        {
            var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);

            if (useOtlpExporter)
            {
                builder.Services.AddOpenTelemetry().UseOtlpExporter();
            }

            // 取消註解以下行以啟用 Azure Monitor 匯出器（需要 Azure.Monitor.OpenTelemetry.AspNetCore 套件）
            //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
            //{
            //    builder.Services.AddOpenTelemetry()
            //       .UseAzureMonitor();
            //}

            return builder;
        }

        /// <summary>
        /// 健康檢查
        /// </summary>        
        public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                // 新增預設的"健康檢查"以確保自己的應用程式能夠回應
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return builder;
        }

        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            // 將運行狀況檢查端點新增至非開發環境中的應用程式會產生安全隱患。
            // 在非開發環境中啟用這些端點之前，請參閱 https://aka.ms/dotnet/aspire/healthchecks 以了解詳細資訊。
            if (app.Environment.IsDevelopment())
            {
                // 所有的運行"健康檢查"必須通過，此應用程式才能在啟動後視為準備好接受傳輸流量
                app.MapHealthChecks("/health");

                // 只有標有「live」標籤的"健康檢查"通過，此應用程式才會被視為處於 live 狀態
                app.MapHealthChecks("/alive", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("live")
                });
            }

            return app;
        }
    }
}
