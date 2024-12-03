#!/bin/bash
# entrypoint.sh

echo "Waiting for MySQL server to become available..."
echo "Install dotnet tools ..."

# Install dotnet-ef tool globally
dotnet tool install --global dotnet-ef

# Ensure dotnet tools are available by adding them to the PATH
export PATH=$PATH:/root/.dotnet/tools

# Debugging: Check the current working directory
echo "Current working directory: $(pwd)"

# Check if the project file exists at the specified path
if [ ! -f ../backen-dotnet.csproj ]; then
  echo "ERROR: Project file backen-dotnet.csproj not found!"
  ls -la
fi

# Sleep for 10 seconds to ensure MySQL is ready
sleep 10

# Apply any pending migrations
echo "Applying migrations..."      
#dotnet ef database update --project /app/backen-dotnet.dll

# Start the application
echo "Starting the application..."
dotnet /app/backen-dotnet.dll