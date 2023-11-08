FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY noteblog/*.csproj ./noteblog/
COPY noteblog/*.config ./noteblog/
RUN nuget restore

# copy everything else and build app
COPY noteblog/. ./noteblog/
WORKDIR /app/noteblog
RUN msbuild /p:Configuration=Release -r:False


FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime
WORKDIR /inetpub/wwwroot
COPY --from=build /app/noteblog/. ./