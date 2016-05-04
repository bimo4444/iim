developer command prompt as administrator (D:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\Tools\VsDevCmd.bat)
netsh http add urlacl url=http://+:666/test user="NT AUTHORITY\NETWORK SERVICE"
installutil c:/*path*/WinService.exe
enjoy

//installutil /u c:/*path*/WinService.exe
//delete urlacl url=http://+:666/test