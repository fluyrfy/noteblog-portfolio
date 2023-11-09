#引入运行环境
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app
# 复制文件
COPY docker/. ./docker/
WORKDIR /app/docker
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS runtime
WORKDIR /inetpub/wwwroot
COPY --from=build /app/docker/. ./