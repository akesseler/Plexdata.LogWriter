@echo off

goto CHOICE_BINARIES

:CHOICE_BINARIES

choice /M "Do you really want to remove all \"bin\" and \"obj\" folders"

if %ERRORLEVEL% == 1 (
  goto CLEAN_BINARIES
) else (
  goto CHOICE_PACKAGES
)

:CLEAN_BINARIES

echo Clean up all "bin" and "obj" folders...

for /d /r %%x in (bin, obj) do rmdir "%%x" /s /q 2> nul

:CHOICE_PACKAGES

choice /M "Do you really want to remove all folders in \"packages\""

if %ERRORLEVEL% == 1 (
  goto CLEAN_PACKAGES
) else (
  goto CHOICE_TRACES
)

:CLEAN_PACKAGES

echo Clean up all "packages"...

rmdir "packages" /s /q 2> nul

:CHOICE_TRACES

choice /M "Do you really want to remove all files \"InternalTrace.*.log\""

if %ERRORLEVEL% == 1 (
  goto CLEAN_TRACES
) else (
  goto CHOICE_FINISHED
)

:CLEAN_TRACES

echo Clean up all "InternalTrace.*.log"...

del InternalTrace.*.log 2> nul

:CHOICE_FINISHED

echo Done!

pause
