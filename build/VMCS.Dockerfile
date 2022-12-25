FROM mcr.microsoft.com/dotnet/sdk:7.0 AS src
WORKDIR /src

COPY ../src .

# Build project
RUN dotnet build VMCS.API

# Publish project
RUN dotnet publish VMCS.API --no-build -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:7.0 as final
WORKDIR /app

COPY --from=src /dist .

ENV ASPNETCORE_URLS=http://*:5001;http://*:5000
EXPOSE 5001 5000

ENTRYPOINT ["dotnet", "VMCS.API.dll"]