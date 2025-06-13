namespace GetDockerContainerEnvironmentParameterExample.Models
{
    public class ContainerEnvironmentModel
    {
        public string AspNetCoreEnvironment =>
                    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;

        public string DotNetEnvironment =>
            Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? string.Empty;

        public string SecurityKey =>
            Environment.GetEnvironmentVariable("security_key") ?? string.Empty;

        public string SecurityKeyHashMAC { get; set; }
    }
}