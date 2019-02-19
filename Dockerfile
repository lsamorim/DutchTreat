FROM microsoft/dotnet AS build-env
WORKDIR /app

# Copiar csproj e restaurar dependencias
COPY ./src/DutchTreat/*.csproj ./
RUN dotnet restore

# Build da aplicacao
COPY . ./
RUN dotnet publish -c Release -o out

# Build da imagem
FROM microsoft/aspnetcore
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "DutchTreat.dll"]