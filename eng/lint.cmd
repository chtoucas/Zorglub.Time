:: Quickly run the "perf" program.

@echo off
@setlocal

@call dotnet fsharplint lint %~dp0\..\test\Zorglub.Tests\Zorglub.Tests.fsproj

@endlocal
@exit /b %ERRORLEVEL%
