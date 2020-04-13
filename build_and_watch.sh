#!/usr/bin/env bash

workspace_folder="$1"

dotnet build "${workspace_folder}/Orga.csproj" "/property:GenerateFullPaths=true" "/consoleloggerparameters:NoSummary"

dotnet watch run "${workspace_folder}/Orga.csproj" "/property:GenerateFullPaths=true" "/consoleloggerparameters:NoSummary"
