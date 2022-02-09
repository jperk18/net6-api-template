FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Health.Patient.Api/Health.Patient.Api.csproj", "Health.Patient.Api/"]
RUN dotnet restore "Health.Patient.Api/Health.Patient.Api.csproj"
COPY . .
WORKDIR "/src/Health.Patient.Api"
RUN dotnet build "Health.Patient.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Health.Patient.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Health.Patient.Grpc/Health.Patient.Grpc.csproj", "Health.Patient.Grpc/"]
RUN dotnet restore "Health.Patient.Grpc/Health.Patient.Grpc.csproj"
COPY . .
WORKDIR "/src/Health.Patient.Grpc"
RUN dotnet build "Health.Patient.Grpc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Health.Patient.Grpc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Health.Patient.Api.dll"]
