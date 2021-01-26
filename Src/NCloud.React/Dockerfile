#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["NCloud.React/NCloud.React.csproj", "NCloud.React/"]
COPY ["NCloud.Core/NCloud.Core.csproj", "NCloud.Core/"]
RUN dotnet restore "NCloud.React/NCloud.React.csproj"
COPY . .
WORKDIR "/src/NCloud.React"
RUN dotnet build "NCloud.React.csproj" -c Release -o /app/build

FROM build AS publish
RUN curl -sL https://deb.nodesource.com/setup_14.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g yarn
RUN dotnet publish "NCloud.React.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "NCloud.React.dll"]