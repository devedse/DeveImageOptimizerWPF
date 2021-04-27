Param(
	[Parameter(Mandatory=$true)]
	[string]$chocolateykey
)

$ErrorActionPreference = "Stop"

Write-Host "Pushing to Chocolatey..."

$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path

$nupkgFile = (Get-ChildItem "$($directorypath)\*" -include *.nupkg).FullName
choco push $nupkgFile -s "https://push.chocolatey.org/" -k $chocolateykey