FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 13497
EXPOSE 44333

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["ServerServerProxy.csproj", ""]
RUN dotnet restore "/ServerServerProxy.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ServerServerProxy.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ServerServerProxy.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ServerServerProxy.dll"]