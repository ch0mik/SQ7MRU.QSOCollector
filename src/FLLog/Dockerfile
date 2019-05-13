FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR ./app
EXPOSE 8421

FROM base AS final
WORKDIR ./app
COPY ./app .
ENTRYPOINT ["dotnet", "SQ7MRU.FLLog.dll"]
