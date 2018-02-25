
set sitename=webapp2.sso.com
set bindings=http/127.0.0.1:80:webapp2.sso.com
set physicalPath=E:\git\github\sso\src\webapplication2

%systemroot%/system32/Inetsrv/APPCMD.exe add site /name:"%sitename%"   /bindings:"%bindings%" /physicalPath:"%physicalPath%"

echo 127.0.0.1 %sitename% >> %SystemRoot%\system32\drivers\etc\hosts

pause