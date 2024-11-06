#!/bin/bash

dotnet ef database update --no-build --project /app/HrDirector/HrDirector.csproj --environment Production

dotnet /app/HrDirector.dll