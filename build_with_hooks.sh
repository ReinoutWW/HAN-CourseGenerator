#!/bin/bash

# Define pre-build command
pre_build_command() {
  echo "Running pre-build command..."
  # Replace with your actual pre-build command
  echo "Executing: Prebuild"
  dotnet sonarscanner begin /k:"ReinoutWW_HAN-CourseGenerator_6e062695-950c-462b-892c-607a5f25b12d" /d:sonar.host.url="http://fraction1.local:30007"  /d:sonar.token="sqp_2a8ecbfd319def4d8e0abd3a9963f8446fb611da"

}

# Define post-build command
post_build_command() {
  echo "Running post-build command..."
  # Replace with your actual post-build command
  echo "Executing: [YOUR_POST_BUILD_COMMAND]"
  dotnet sonarscanner end /d:sonar.token="sqp_2a8ecbfd319def4d8e0abd3a9963f8446fb611da" 
}

# Define your build command
build_command() {
  echo "Starting the build process..."
  # Replace with your actual build command
  echo "Executing: [YOUR_BUILD_COMMAND]"
  dotnet build
}

# Check for arguments
if [ $# -eq 0 ]; then
  echo "No arguments supplied. Usage: $0 [build_argument]"
  exit 1
fi

# Main script logic
if [ "$1" == "SonarScan" ]; then
  echo "Build argument detected: $1"

  # Run pre-build command
  pre_build_command

  # Run the build command
  build_command

  # Run post-build command
  post_build_command
else
  echo "Unknown argument: $1"
  exit 1
fi
