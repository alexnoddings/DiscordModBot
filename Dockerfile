#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Elvet.Main/Elvet.Main.csproj", "Elvet.Main/"]
RUN dotnet restore "Elvet.Main/Elvet.Main.csproj"
COPY . .
WORKDIR "/src/Elvet.Main"
RUN dotnet build "Elvet.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elvet.Main.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elvet.Main.dll"]