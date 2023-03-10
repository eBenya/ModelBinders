FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["../Examples/JsonWithFilesBindingExample/JsonWithFilesBindingExample.csproj", "../Examples/JsonWithFilesBindingExample/"]
RUN dotnet restore "../Examples/JsonWithFilesBindingExample/JsonWithFilesBindingExample.csproj"
COPY . .
WORKDIR "/src/../Examples/JsonWithFilesBindingExample"
RUN dotnet build "JsonWithFilesBindingExample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JsonWithFilesBindingExample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JsonWithFilesBindingExample.dll"]
