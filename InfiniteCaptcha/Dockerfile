FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["InfiniteCaptcha.Api/InfiniteCaptcha.Api.csproj", "InfiniteCaptcha.Api/"]
COPY ["InfiniteCaptcha.Shared/InfiniteCaptcha.Shared.csproj", "InfiniteCaptcha.Shared/"]
RUN dotnet restore "InfiniteCaptcha.Api/InfiniteCaptcha.Api.csproj"

COPY . .
WORKDIR "/src/InfiniteCaptcha.Api"
RUN dotnet publish "InfiniteCaptcha.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "InfiniteCaptcha.Api.dll"]