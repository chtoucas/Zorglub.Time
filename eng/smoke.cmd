:: Quickly run the tests.

@echo off
@setlocal

@call dotnet test --no-build %~dp0\..\test\Zorglub.Tests\ ^
    --filter "ExcludeFrom!=Smoke&Performance!~Slow&Redundant!=true" -- %*

@endlocal
@exit /b %ERRORLEVEL%
