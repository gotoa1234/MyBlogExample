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
    // �s�W�`���� .NET Aspire �A�ȡG�A�ȵo�{�B�u�ʡB�B�檬�p�ˬd�M OpenTelemetry�C
    // �z���ѨM��פ����C�ӪA�ȶ��س����ޥθӶ��ءC
    // �n�F�Ѧ����ϥΦ����ت���h�H���A�аѾ\ https://aka.ms/dotnet/aspire/service-defaults
    public static class Extensions
    {
        public static IHostApplicationBuilder AddServiceDefaults(this IHostApplicationBuilder builder)
        {
            builder.ConfigureOpenTelemetry();

            builder.AddDefaultHealthChecks();

            builder.Services.AddServiceDiscovery();

            builder.Services.ConfigureHttpClientDefaults(http =>
            {
                // �w�]�}�� �u�ʳB�z�{��
                http.AddStandardResilienceHandler();

                // �w�]�}�� �A�ȵo�{
                http.AddServiceDiscovery();
            });

            // �����H�U�������e�i����"���\"���A�ȵo�{��סC
            // builder.Services.Configure<ServiceDiscoveryOptions>(options =>
            // {
            //     options.AllowedSchemes = ["https"];
            // });

            return builder;
        }

        /// <summary>
        /// �t�m�������l�ܩM���Ц����A���\���ε{�����ʱ��M�i�[��ʡC
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
                        // ���������H�U��H�ҥ� gRPC �˴��]�ݭn OpenTelemetry.Instrumentation.GrpcNetClient �M��^
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

            // �������ѥH�U��H�ҥ� Azure Monitor �ץX���]�ݭn Azure.Monitor.OpenTelemetry.AspNetCore �M��^
            //if (!string.IsNullOrEmpty(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]))
            //{
            //    builder.Services.AddOpenTelemetry()
            //       .UseAzureMonitor();
            //}

            return builder;
        }

        /// <summary>
        /// ���d�ˬd
        /// </summary>        
        public static IHostApplicationBuilder AddDefaultHealthChecks(this IHostApplicationBuilder builder)
        {
            builder.Services.AddHealthChecks()
                // �s�W�w�]��"���d�ˬd"�H�T�O�ۤv�����ε{������^��
                .AddCheck("self", () => HealthCheckResult.Healthy(), ["live"]);

            return builder;
        }

        public static WebApplication MapDefaultEndpoints(this WebApplication app)
        {
            // �N�B�檬�p�ˬd���I�s�W�ܫD�}�o���Ҥ������ε{���|���ͦw�����w�C
            // �b�D�}�o���Ҥ��ҥγo�Ǻ��I���e�A�аѾ\ https://aka.ms/dotnet/aspire/healthchecks �H�F�ѸԲӸ�T�C
            if (app.Environment.IsDevelopment())
            {
                // �Ҧ����B��"���d�ˬd"�����q�L�A�����ε{���~��b�Ұʫ�����ǳƦn�����ǿ�y�q
                app.MapHealthChecks("/health");

                // �u���Ц��ulive�v���Ҫ�"���d�ˬd"�q�L�A�����ε{���~�|�Q�����B�� live ���A
                app.MapHealthChecks("/alive", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("live")
                });
            }

            return app;
        }
    }
}
