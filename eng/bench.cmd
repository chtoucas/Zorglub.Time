:: Quickly run the "perf" program.

@echo off
@setlocal

@rem dotnet run -c Release -f net48 -filter "**" --runtimes net48 net5.0 net6.0
@call dotnet run -c Release --project %~dp0\..\tools\Benchmarks\ ^
    -p:AnalysisMode=AllDisabledByDefault -- %*

@endlocal
@exit /b %ERRORLEVEL%
