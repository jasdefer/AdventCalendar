#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/aspnet/sdk:6.0-bullseye AS build
WORKDIR /src
COPY ["Source/AdventCalendarWebApp/AdventCalendarWebApp.csproj", "Source/AdventCalendarWebApp/"]
RUN dotnet restore "Source/AdventCalendarWebApp/AdventCalendarWebApp.csproj"
COPY . .
WORKDIR "/src/Source/AdventCalendarWebApp"
RUN dotnet build "AdventCalendarWebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdventCalendarWebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdventCalendarWebApp.dll"]