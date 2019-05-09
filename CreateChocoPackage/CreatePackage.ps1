$fileToPackage = "Scripts\Output\DeveImageOptimizerWPF.exe"

$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path
$SolutionRoot = Split-Path -Path $directorypath -Parent

$fullPathFileToPackage = Join-Path $SolutionRoot $fileToPackage -Resolve
$toolsDir = Join-Path $directorypath "tools"

$destinationFileName = [System.IO.Path]::GetFileName($fullPathFileToPackage)
$destinationFilePath = Join-Path $toolsDir $destinationFileName
$chocoinstallpath = Join-path $toolsDir "chocolateyinstall.ps1"

Copy-Item $fullPathFileToPackage $destinationFilePath


$ReleaseVersionNumberFull = (Get-Item $destinationFilePath).VersionInfo.FileVersion
$checksum = checksum -t sha256 -f $destinationFilePath

$nuspecFile = (Get-ChildItem "$($directorypath)\*" -include *.nuspec).FullName


$re = [regex]"(?<=<version>).*(?=<\/version>)"
$reChecksum = [regex]"{checksumtoreplace}"

Write-Host "Setting version $ReleaseVersionNumberFull in $nuspecFile by using Regex: $re"
 
$re.Replace([string]::Join("`n", (Get-Content -Path $nuspecFile)), "$ReleaseVersionNumberFull", 1) | Set-Content -Path $nuspecFile -Encoding UTF8

Write-Host "Setting checksum $checksum in $chocoinstallpath by using Regex: $reChecksum"

$reChecksum.Replace([string]::Join("`n", (Get-Content -Path $chocoinstallpath)), "$checksum", 1) | Set-Content -Path $chocoinstallpath -Encoding UTF8

Set-Location -Path $directorypath
choco pack