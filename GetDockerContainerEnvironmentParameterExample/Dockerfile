FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ENV ASPNETCORE_ENVIRONMENT=DEV
ENV DOTNET_ENVIRONMENT=DEV
ENV security_key=DEVTestKey

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY ./publish .

RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' > /etc/timezone

ENTRYPOINT ["dotnet", "GetDockerContainerEnvironmentParameterExample.dll"]
