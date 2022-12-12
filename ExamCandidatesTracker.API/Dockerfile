#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ExamCandidatesTracker.API/ExamCandidatesTracker.API.csproj", "ExamCandidatesTracker.API/"]
COPY ["ExamCandidatesTracker.Core/ExamCandidatesTracker.Core.csproj", "ExamCandidatesTracker.Core/"]
COPY ["ExamCandidatesTracker.Domain/ExamCandidatesTracker.Domain.csproj", "ExamCandidatesTracker.Domain/"]
COPY ["ExamCandidatesTracker.Infrastructure/ExamCandidatesTracker.Infrastructure.csproj", "ExamCandidatesTracker.Infrastructure/"]
RUN dotnet restore "ExamCandidatesTracker.API/ExamCandidatesTracker.API.csproj"
COPY . .
WORKDIR "/src/ExamCandidatesTracker.API"
RUN dotnet build "ExamCandidatesTracker.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExamCandidatesTracker.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=publish /src/ExamCandidatesTracker.API/examcandidatestracker.pfx ./
ENTRYPOINT ["dotnet", "ExamCandidatesTracker.API.dll"]