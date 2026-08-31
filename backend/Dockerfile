FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Blacklabel.sln .
COPY src/Blacklabel.Api/Blacklabel.Api.csproj src/Blacklabel.Api/
COPY src/Blacklabel.Application/Blacklabel.Application.csproj src/Blacklabel.Application/
COPY src/Blacklabel.Domain/Blacklabel.Domain.csproj src/Blacklabel.Domain/
COPY src/Blacklabel.Infrastructure/Blacklabel.Infrastructure.csproj src/Blacklabel.Infrastructure/
RUN dotnet restore src/Blacklabel.Api/Blacklabel.Api.csproj

COPY src/ src/
RUN dotnet publish src/Blacklabel.Api/Blacklabel.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Blacklabel.Api.dll"]
