$ErrorActionPreference = "Stop"

if ($env:APPVEYOR_REPO_BRANCH -eq "master") {
	Write-Host "Pushing to Chocolatey..."

	$invocation = (Get-Variable MyInvocation).Value
	$directorypath = Split-Path $invocation.MyCommand.Path

	$nupkgFile = (Get-ChildItem "$($directorypath)\*" -include *.nupkg).FullName
	choco push $nupkgFile -s "https://push.chocolatey.org/" -k $env:CHOCOLATEYKEY
} else {
	Write-Host "Skipping Chocolatey Push because we're not on the master branch"
}