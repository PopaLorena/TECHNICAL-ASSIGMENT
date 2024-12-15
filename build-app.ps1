# build-app.ps1

# Display the start of the build process
Write-Host "Starting the build process..."

# Restore dependencies
Write-Host "Restoring dependencies..."
dotnet restore

# Build the application
Write-Host "Building the application..."
dotnet build ./Assigment/Assigment.sln --configuration Release

# Run tests (optional, if you have test projects)
Write-Host "Running tests..."
dotnet test ./Assigment/Assigment.sln --configuration Release


Write-Host "Build process completed successfully."