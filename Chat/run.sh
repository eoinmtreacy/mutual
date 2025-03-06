#!/bin/bash

SERVICE_NAME=Chat

# Find and kill the running process for the service
PID=$(pgrep -f "$SERVICE_NAME.dll")

if [ -n "$PID" ]; then
  echo "Stopping $SERVICE_NAME (PID: $PID)"
  kill "$PID"
else
  echo "No running process found for $SERVICE_NAME"
fi

# Clean up old publish directory
echo "Cleaning up"
dotnet clean

# Publish the .NET project
echo "Rebuilding $SERVICE_NAME"
dotnet publish 

# Run the newly published DLL
echo "Starting $SERVICE_NAME..."
nohup dotnet "./bin/Release/net9.0/$SERVICE_NAME.dll" >> ./logs 2>&1 &

echo "$SERVICE_NAME deployed and running!"



