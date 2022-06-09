#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0-focal-arm64v8 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal-arm64v8 AS build
WORKDIR /src
COPY ["Un1ver5e.Bot/Un1ver5e.Bot.csproj", "Un1ver5e.Bot/"]
COPY ["Un1ver5e.Bot/NuGet.Config", "Un1ver5e.Bot"]
RUN dotnet restore "Un1ver5e.Bot/Un1ver5e.Bot.csproj"
COPY . .
WORKDIR "/src/Un1ver5e.Bot"
RUN dotnet build "Un1ver5e.Bot.csproj" -c Release -o /app/build -r linux-arm64

FROM build AS publish
RUN dotnet publish "Un1ver5e.Bot.csproj" -c Release -o /app/publish -r linux-arm64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Un1ver5e.Bot.dll"]
COPY ["../Data", "Data"]