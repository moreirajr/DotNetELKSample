#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DotNetELKSample.Api/DotNetELKSample.Api.csproj", "DotNetELKSample.Api/"]
COPY ["DotNetELKSample.Serilog/DotNetELKSample.Serilog.csproj", "DotNetELKSample.Serilog/"]
RUN dotnet restore "DotNetELKSample.Api/DotNetELKSample.Api.csproj"
COPY . .
WORKDIR "/src/DotNetELKSample.Api"
RUN dotnet build "DotNetELKSample.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNetELKSample.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetELKSample.Api.dll"]