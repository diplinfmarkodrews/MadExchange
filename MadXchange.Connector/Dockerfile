#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MadXchange.Connector/MadXchange.Connector.csproj", "MadXchange.Connector/"]
COPY ["MadXchange.Exchange/MadXchange.Exchange.csproj", "MadXchange.Exchange/"]
COPY ["MadXchange.Common/MadXchange.Common.csproj", "MadXchange.Common/"]
RUN dotnet restore "MadXchange.Connector/MadXchange.Connector.csproj"
COPY . .
WORKDIR "/src/MadXchange.Connector"
RUN dotnet build "MadXchange.Connector.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MadXchange.Connector.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MadXchange.Connector.dll"]