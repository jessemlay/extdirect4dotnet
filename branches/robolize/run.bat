SET webRootPath=build\extdirect4dotnet-Release\WebSamples
SET port=1230
SET serverExe=WebDev.WebServer40.EXE
SET commonFiles=%ProgramFiles%\Common Files

IF EXIST %ProgramFiles(x86)% (
	SET commonFiles=%ProgramFiles(x86)%\Common Files
) 

TASKKILL /F /IM %serverExe%
START /D "%commonFiles%\microsoft shared\DevServer\10.0\" /B %serverExe% /port:%port% /path:"%CD%\%webRootPath%" /vpath:"/"

START /b "C:\Program Files\Internet Explorer\iexplore.exe" "http://localhost:%port%/Default.aspx"
