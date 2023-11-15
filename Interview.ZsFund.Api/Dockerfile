FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Interview.ZsFund.Api/Interview.ZsFund.Api.csproj", "Interview.ZsFund.Api/"]
RUN dotnet restore "Interview.ZsFund.Api/Interview.ZsFund.Api.csproj"
COPY . .
WORKDIR "/src/Interview.ZsFund.Api"
RUN dotnet build "Interview.ZsFund.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Interview.ZsFund.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Interview.ZsFund.Api.dll"]
