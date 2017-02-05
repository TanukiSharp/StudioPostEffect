@echo off

rmdir /S /Q "StudioPostEffect\bin"

"%WINDIR%\Microsoft.NET\Framework\v3.5\msbuild.exe" StudioPostEffect.sln /t:Rebuild /p:Configuration=Release

mkdir "Delivery\StudioPostEffect"
mkdir "Delivery\StudioPostEffect\Samples"
mkdir "Delivery\StudioPostEffect\Samples\BaseSamples"
mkdir "Delivery\StudioPostEffect\Samples\CodePlex Shaders"
mkdir "Delivery\StudioPostEffect\Samples\Ogre Shaders"
mkdir "Delivery\StudioPostEffect\Samples\PhotoShop Filters"

del "StudioPostEffect\bin\Release\*.pdb"
del "StudioPostEffect\bin\Release\StudioPostEffect.vshost.exe"
del "StudioPostEffect\bin\Release\plugins\*.loaded"

xcopy /Y /E /C /R "StudioPostEffect\bin\Release\*" "Delivery\StudioPostEffect"
xcopy /Y /E /C /R "Resources\runtime\*" "Delivery\StudioPostEffect"
xcopy /Y /E /C /R "Samples\BaseSamples\*" "Delivery\StudioPostEffect\Samples\BaseSamples"
xcopy /Y /E /C /R "Samples\CodePlex Shaders\*" "Delivery\StudioPostEffect\Samples\CodePlex Shaders\"
xcopy /Y /E /C /R "Samples\Ogre Shaders\*" "Delivery\StudioPostEffect\Samples\Ogre Shaders\"
xcopy /Y /E /C /R "Samples\PhotoShop Filters\*" "Delivery\StudioPostEffect\Samples\PhotoShop Filters\"

del "Delivery\StudioPostEffect\*.pdb"
del "Delivery\StudioPostEffect\StudioPostEffect.vshost.exe"
del "Delivery\StudioPostEffect\plugins\*.loaded"

pause
