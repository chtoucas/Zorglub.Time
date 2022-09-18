:: Quickly run the "perf" program.

@echo off
@setlocal

@call dotnet fsharplint lint -l %~dp0\fsharplint.json %~dp0\..\test\Zorglub.Tests\Zorglub.Tests.fsproj

@endlocal
@exit /b %ERRORLEVEL%
