#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GRSMU.Bot.Api/GRSMU.Bot.Api.csproj", "src/GRSMU.Bot.Api/"]
RUN dotnet restore "src/GRSMU.Bot.Api/GRSMU.Bot.Api.csproj"
COPY . .
WORKDIR "/src/src/GRSMU.Bot.Api"
RUN dotnet build "GRSMU.Bot.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GRSMU.Bot.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GRSMU.Bot.Api.dll"]