#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Host/Elvet.Host.csproj", "Host/"]
COPY ["Core/Elvet.Core.csproj", "Core/"]
COPY ["Modules.RoleBack/Elvet.RoleBack.csproj", "Modules.RoleBack/"]
RUN dotnet restore "Host/Elvet.Host.csproj"
COPY . .
WORKDIR "/src/Host"
RUN dotnet build "Elvet.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elvet.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elvet.Host.dll"]