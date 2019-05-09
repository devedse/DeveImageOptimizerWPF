$fileToPackage = "Scripts\Output\DeveImageOptimizerWPF.exe"

$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path
$CreateChocoDir = Split-Path -Path $directorypath -Parent
$SolutionRoot = Split-Path -Path $CreateChocoDir -Parent

$fullPathFileToPackage = Join-Path $SolutionRoot $fileToPackage -Resolve
$destinationFileName = [System.IO.Path]::GetFileName($fullPathFileToPackage)
$destinationFilePath = "$($directorypath)\tools\$($destinationFileName)"

Copy-Item $fullPathFileToPackage $destinationFilePath


$ReleaseVersionNumberFull = (Get-Item $destinationFilePath).VersionInfo.FileVersion

$nuspecFile = (Get-ChildItem "$($directorypath)\*" -include *.nuspec).FullName


$re = [regex]"(?<=<version>).*(?=<\/version>)"

Write-Host "Regex1: $re"
Write-Host "Setting version $ReleaseVersionNumberFull in $nuspecFile"
 
$re.Replace([string]::Join("`n", (Get-Content -Path $nuspecFile)), "$ReleaseVersionNumberFull", 1) | Set-Content -Path $nuspecFile -Encoding UTF8