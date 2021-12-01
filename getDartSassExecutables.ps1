$DartSassVersion = Select-Xml -Path .\build\runtime.props -XPath /Project/PropertyGroup/Version | ForEach-Object { $_.Node.InnerXml }
$DownloadUrl = "https://github.com/sass/dart-sass/releases/download/$($DartSassVersion)/"
$TempDir = "./dart-sass/temp"

New-Item -ItemType Directory -Force $TempDir

Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-windows-x64.zip" -OutFile "$TempDir/win-x64.zip"
Expand-Archive -Path "$TempDir/win-x64.zip" -DestinationPath "./dart-sass/win-x64" -Force

Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-windows-ia32.zip" -OutFile "$TempDir/win-ia32.zip"
Expand-Archive -Path "$TempDir/win-ia32.zip" -DestinationPath "./dart-sass/win-ia32" -Force

Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-linux-x64.tar.gz" -OutFile "$TempDir/linux-x64.tar.gz"
New-Item -ItemType Directory -Force "./dart-sass/linux-x64"
tar -xf "$TempDir/linux-x64.tar.gz" -C "./dart-sass/linux-x64"

Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-linux-ia32.tar.gz" -OutFile "$TempDir/linux-ia32.tar.gz"
New-Item -ItemType Directory -Force "./dart-sass/linux-ia32"
tar -xf "$TempDir/linux-ia32.tar.gz" -C "./dart-sass/linux-ia32"

Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-macos-x64.tar.gz" -OutFile "$TempDir/macos-x64.tar.gz"
New-Item -ItemType Directory -Force "./dart-sass/macos-x64"
tar -xf "$TempDir/macos-x64.tar.gz" -C "./dart-sass/macos-x64"

Remove-Item -Path $TempDir -Recurse