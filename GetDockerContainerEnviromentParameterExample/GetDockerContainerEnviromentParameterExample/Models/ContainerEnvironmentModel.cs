namespace GetDockerContainerEnviromentParameterExample.Models
{
    /// <summary>
    /// 容器的環境變數
    /// </summary>
    public class ContainerEnvironmentModel
    {
        public string ASPNETCORE_ENVIRONMENT 
        { 
            get {
                return Environment.GetEnvironmentVariable(nameof(ASPNETCORE_ENVIRONMENT)) ?? string.Empty; 
            }
        }

        public string DOTNET_ENVIRONMENT
        {
            get
            {
                return Environment.GetEnvironmentVariable(nameof(DOTNET_ENVIRONMENT)) ?? string.Empty;
            }
        }

        public string security_key
        {
            get
            {
                return Environment.GetEnvironmentVariable(nameof(security_key)) ?? string.Empty;
            }
        }
    }
}
