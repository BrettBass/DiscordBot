FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /src

# Copy main project
COPY *.csproj .
RUN dotnet restore

# Copy and publish app and libraries
COPY . .

RUN dotnet build

# Run the project
ENTRYPOINT [ "dotnet", "run" ]