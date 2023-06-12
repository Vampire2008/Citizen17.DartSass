$DartSassVersion = Select-Xml -Path .\build\runtime.props -XPath /Project/PropertyGroup/Version | ForEach-Object { $_.Node.InnerXml }
$DownloadUrl = "https://github.com/sass/dart-sass/releases/download/$($DartSassVersion)/"
$TempDir = "./dart-sass/temp"

Remove-Item "./dart-sass" -Recurse
New-Item -ItemType Directory -Force $TempDir

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-windows-x64.zip" -OutFile "$TempDir/win-x64.zip"
	Expand-Archive -Path "$TempDir/win-x64.zip" -DestinationPath "./dart-sass/win-x64" -Force
} catch {
	Write-Host "Problem with Windows x64";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-windows-ia32.zip" -OutFile "$TempDir/win-ia32.zip"
	Expand-Archive -Path "$TempDir/win-ia32.zip" -DestinationPath "./dart-sass/win-ia32" -Force
} catch {
	Write-Host "Problem with Windows x86";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-linux-x64.tar.gz" -OutFile "$TempDir/linux-x64.tar.gz"
	New-Item -ItemType Directory -Force "./dart-sass/linux-x64"
	tar -xf "$TempDir/linux-x64.tar.gz" -C "./dart-sass/linux-x64"
} catch {
	Write-Host "Problem with Linux x64";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-linux-ia32.tar.gz" -OutFile "$TempDir/linux-ia32.tar.gz"
	New-Item -ItemType Directory -Force "./dart-sass/linux-ia32"
	tar -xf "$TempDir/linux-ia32.tar.gz" -C "./dart-sass/linux-ia32"
} catch {
	Write-Host "Problem with Linux x86";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-linux-arm.tar.gz" -OutFile "$TempDir/linux-arm.tar.gz"
	New-Item -ItemType Directory -Force "./dart-sass/linux-arm"
	tar -xf "$TempDir/linux-arm.tar.gz" -C "./dart-sass/linux-arm"
} catch {
	Write-Host "Problem with Linux arm";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-linux-arm64.tar.gz" -OutFile "$TempDir/linux-arm64.tar.gz"
	New-Item -ItemType Directory -Force "./dart-sass/linux-arm64"
	tar -xf "$TempDir/linux-arm64.tar.gz" -C "./dart-sass/linux-arm64"
} catch {
	Write-Host "Problem with Linux arm64";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-macos-x64.tar.gz" -OutFile "$TempDir/macos-x64.tar.gz"
	New-Item -ItemType Directory -Force "./dart-sass/macos-x64"
	tar -xf "$TempDir/macos-x64.tar.gz" -C "./dart-sass/macos-x64"
} catch {
	Write-Host "Problem with MacOS x64";
	Write-Host $_
}

try {
	Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-macos-arm64.tar.gz" -OutFile "$TempDir/macos-arm64.tar.gz"
	New-Item -ItemType Directory -Force "./dart-sass/macos-arm64"
	tar -xf "$TempDir/macos-arm64.tar.gz" -C "./dart-sass/macos-arm64"
} catch {
	Write-Host "Problem with MacOS arm64";
	Write-Host $_
}

Remove-Item -Path $TempDir -Recurse
