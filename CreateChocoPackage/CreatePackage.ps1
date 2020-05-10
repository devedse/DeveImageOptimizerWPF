$ErrorActionPreference = "Stop"

$fileToPackage = "DeveImageOptimizerWPF\bin\Release\netcoreapp3.1\win-x64\publish\DeveImageOptimizerWPF.exe"

#Install-Module -Name FormatPowerShellCode -Scope CurrentUser -Confirm:$False

$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path
$SolutionRoot = Split-Path -Path $directorypath -Parent

$powerShellHelpersModule = Join-Path $directorypath "PowerShellHelpers"
Import-Module -Name $powerShellHelpersModule

$fullPathFileToPackage = Join-Path $SolutionRoot $fileToPackage -Resolve
$toolsDir = Join-Path $directorypath "tools"
$templateDir = Join-Path $directorypath "templates"

$destinationFileName = [System.IO.Path]::GetFileName($fullPathFileToPackage)
$destinationFileNameNoExtension = [System.IO.Path]::GetFileNameWithoutExtension($fullPathFileToPackage)
$destinationFilePath = Join-Path $toolsDir $destinationFileName
$destinationFilePath7z = Join-path $toolsDir "$($destinationFileNameNoExtension).7z"
$chocoinstallpsfilepath = Join-path $toolsDir "chocolateyinstall.ps1"
$chocouninstallpsfilepath = Join-path $toolsDir "chocolateyuninstall.ps1"
$chocobeforemodifypsfilepath = Join-path $toolsDir "chocolateybeforemodify.ps1"
$verificationfilepath = Join-Path $toolsDir "VERIFICATION.txt"

$chocoinstallpsfiletemplatepath = Join-path $templateDir "chocolateyinstall.ps1"
$chocouninstallpsfiletemplatepath = Join-path $templateDir "chocolateyuninstall.ps1"
$chocobeforemodifypsfiletemplatepath = Join-path $templateDir "chocolateybeforemodify.ps1"
$verificationfiletemplatepath = Join-Path $templateDir "VERIFICATION.txt"

if (Test-Path $destinationFilePath) 
{
    Remove-Item $destinationFilePath
}
if (Test-Path $destinationFilePath7z) 
{
    Remove-Item $destinationFilePath7z
}

7z a -t7z -m0=LZMA2 -mmt=on -mx9 -md=1536m -mfb=273 -ms=on -mqs=on -sccUTF-8 "$destinationFilePath7z" "$fullPathFileToPackage"
#Copy-Item $fullPathFileToPackage $destinationFilePath

Write-Host "Copying templates..."

Copy-Item $chocoinstallpsfiletemplatepath $chocoinstallpsfilepath
#Copy-Item $chocouninstallpsfiletemplatepath $chocouninstallpsfilepath
#Copy-Item $chocobeforemodifypsfiletemplatepath $chocobeforemodifypsfilepath
Copy-Item $verificationfiletemplatepath $verificationfilepath

Write-Host "Removing comments..."
Remove-Comments($chocoinstallpsfilepath)
#Remove-Comments($chocouninstallpsfilepath)
#Remove-Comments($chocobeforemodifypsfilepath)


$ReleaseVersionNumberFull = (Get-Item $fullPathFileToPackage).VersionInfo.FileVersion
$ReleaseVersionNumberShort = $ReleaseVersionNumberFull.Substring(0, $ReleaseVersionNumberFull.LastIndexOf('.'))
$checksum = checksum -t sha256 -f $fullPathFileToPackage
#$checksum = checksum -t sha256 -f $destinationFilePath7z

$nuspecFile = (Get-ChildItem "$($directorypath)\*" -include *.nuspec).FullName


$re = [regex]"(?<=<version>).*(?=<\/version>)"
$reChecksum = [regex]"{checksumtoreplace}"

Write-Host "Setting version $ReleaseVersionNumberShort in $nuspecFile by using Regex: $re"
 
$re.Replace([string]::Join("`n", (Get-Content -Path $nuspecFile)), "$ReleaseVersionNumberShort", 1) | Set-Content -Path $nuspecFile -Encoding UTF8

Write-Host "Setting checksum $checksum in $verificationfilepath by using Regex: $reChecksum"

$reChecksum.Replace([string]::Join("`n", (Get-Content -Path $verificationfilepath)), "$checksum", 1) | Set-Content -Path $verificationfilepath -Encoding UTF8

Push-Location -Path $directorypath
choco pack
Pop-Location
