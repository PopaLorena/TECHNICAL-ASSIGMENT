@echo off

setlocal enabledelayedexpansion
set ERRORS=0

set SOLUTION=Assigment\Assigment.sln

set CONFIGURATION=Release

dotnet --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] .NET CLI is not installed or not available in PATH.
    set /a ERRORS+=1
    goto END
)

cd /d "%~dp0"

echo [INFO] Cleaning previous build...
dotnet clean "%SOLUTION%" --configuration %CONFIGURATION%
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Cleaning the solution failed.
    set /a ERRORS+=1
    goto END
)

echo [INFO] Restoring dependencies...
dotnet restore "%SOLUTION%"
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Restoring dependencies failed.
    set /a ERRORS+=1
    goto END
)

echo [INFO] Building the solution...
dotnet build "%SOLUTION%" --configuration %CONFIGURATION%
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Building the solution failed.
    set /a ERRORS+=1
    goto END
)

:END
if %ERRORS% EQU 0 (
    echo [SUCCESS] Build completed successfully!
    exit /b 0
) else (
    echo [FAILURE] Build failed with %ERRORS% error(s).
    exit /b 1
)
