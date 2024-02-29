$DartSassVersion = Select-Xml -Path .\build\runtime.props -XPath /Project/PropertyGroup/Version | ForEach-Object { $_.Node.InnerXml }
$DownloadUrl = "https://github.com/sass/dart-sass/releases/download/$($DartSassVersion)/"
$TempDir = "./dart-sass/temp"

if (Test-Path "./dart-sass") {
	Remove-Item "./dart-sass" -Recurse
}
New-Item -ItemType Directory -Force $TempDir

function downloadAndExtract($name, $title) {
	try {
		Invoke-WebRequest -Uri "$($DownloadUrl)dart-sass-$($DartSassVersion)-$($name)" -OutFile "$TempDir/$name"
		if ($name.EndsWith("tar.gz")) {
			$dirName = [System.IO.Path]::GetFileNameWithoutExtension([System.IO.Path]::GetFileNameWithoutExtension($name));
			New-Item -ItemType Directory -Force "./dart-sass/$dirName"
			tar -xf "$TempDir/$name" -C "./dart-sass/$dirName"
		}
		else {
			$dirName = [System.IO.Path]::GetFileNameWithoutExtension($name)
			Expand-Archive -Path "$TempDir/$name" -DestinationPath "./dart-sass/$dirName" -Force
		}
	} catch {
		Write-Host "Problem with $title";
		Write-Host $_
	}
}

downloadAndExtract "windows-x64.zip"         "Windows x64"
downloadAndExtract "windows-ia32.zip"        "Windows x86"

downloadAndExtract "linux-x64.tar.gz"        "Linux x64"
downloadAndExtract "linux-ia32.tar.gz"       "Linux x86"
downloadAndExtract "linux-arm.tar.gz"        "Linux arm"
downloadAndExtract "linux-arm64.tar.gz"      "Linux arm64"

downloadAndExtract "linux-x64-musl.tar.gz"   "Linux musl x64"
downloadAndExtract "linux-ia32-musl.tar.gz"  "Linux musl x86"
downloadAndExtract "linux-arm-musl.tar.gz"   "Linu muslx arm"
downloadAndExtract "linux-arm64-musl.tar.gz" "Linux musl arm64"

downloadAndExtract "macos-x64.tar.gz"        "MacOS x64"
downloadAndExtract "macos-arm64.tar.gz"      "MacOS arm64"

downloadAndExtract "android-x64.tar.gz"      "Android x64"
downloadAndExtract "android-ia32.tar.gz"     "Android x86"
downloadAndExtract "android-arm.tar.gz"      "Android arm"
downloadAndExtract "android-arm64.tar.gz"    "Android arm64"

Remove-Item -Path $TempDir -Recurse
