# ���������� ����������� ����� ASP.NET Core ��� ������� ����������
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# ���������� SDK ����� ��� ������ ����������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# �������� .csproj � ��������������� �����������
COPY ["Tutor.csproj", "./"]
RUN dotnet restore "Tutor.csproj"

# �������� ��� ����� � �������� ����������
COPY . .
RUN dotnet build "Tutor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ��������� ����������
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Tutor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ��������� ����� ��� �������
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tutor.dll"]
