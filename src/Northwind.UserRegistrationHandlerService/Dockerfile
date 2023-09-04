#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Northwind.UserRegistrationHandlerService/Northwind.UserRegistrationHandlerService.csproj", "src/Northwind.UserRegistrationHandlerService/"]
COPY ["src/Northwind.Contracts/Northwind.Contracts.csproj", "src/Northwind.Contracts/"]
RUN dotnet restore "src/Northwind.UserRegistrationHandlerService/Northwind.UserRegistrationHandlerService.csproj"
COPY . .
WORKDIR "/src/src/Northwind.UserRegistrationHandlerService"
RUN dotnet build "Northwind.UserRegistrationHandlerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Northwind.UserRegistrationHandlerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Northwind.UserRegistrationHandlerService.dll"]