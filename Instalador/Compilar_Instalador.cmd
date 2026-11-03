@echo off
rem Compila la solucion en Release y genera Instalador\bin\AeroManager_Setup.msi
rem Requiere Visual Studio y WiX Toolset 3.14. Antes de cada entrega, subir Version en Services_SetUp.wxs.
setlocal
cd /d "%~dp0.."

set "VSWHERE=%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe"
for /f "usebackq delims=" %%i in (`"%VSWHERE%" -latest -requires Microsoft.Component.MSBuild -find MSBuild\**\Bin\MSBuild.exe`) do set "MSBUILD=%%i"
if not defined MSBUILD (
    echo No se encontro MSBuild.
    exit /b 1
)

"%MSBUILD%" ServicesIngSW_23DB.slnx /t:Build /p:Configuration=Release /v:minimal /nologo
if errorlevel 1 exit /b 1

rem candle y light los ejecuta MSBuild a traves del .wixproj, que ya declara
rem WixUIExtension y WixNetFxExtension (esta ultima para la condicion de .NET 4.7.2).
"%MSBUILD%" Services_SetUp.wixproj /t:Rebuild /p:Configuration=Release /v:minimal /nologo
if errorlevel 1 exit /b 1

echo.
echo Instalador generado en Instalador\bin\AeroManager_Setup.msi
