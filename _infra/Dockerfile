# Handy to keep this one around for doing expirements on other platforms :-)

FROM mcr.microsoft.com/dotnet/sdk:6.0 as BUILDER

WORKDIR /app

COPY Blog.csproj Blog.csproj
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /artifact

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as RUNNER

WORKDIR /app
COPY --from=BUILDER /artifact .

CMD ["dotnet","Blog.dll"]
