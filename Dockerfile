FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
# Build da aplicacao
COPY ./src ./
# RUN ls -l
WORKDIR /app/DutchTreat
# RUN ls -l
RUN dotnet restore
RUN dotnet publish -c Release -o out
COPY ./src/DutchTreat/node_modules ./out/node_modules
# RUN ls -l

# Build da imagem
FROM microsoft/dotnet:2.2-aspnetcore-runtime
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
WORKDIR /app
COPY --from=build-env /app/DutchTreat/out .
# RUN ls -l
ENTRYPOINT ["dotnet", "DutchTreat.dll"]
