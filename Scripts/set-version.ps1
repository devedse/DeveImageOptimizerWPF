$ReleaseVersionNumber = $env:APPVEYOR_BUILD_VERSION
$PreReleaseName = ''

If($env:APPVEYOR_PULL_REQUEST_NUMBER -ne $null) {
  $PreReleaseName = '-PR-' + $env:APPVEYOR_PULL_REQUEST_NUMBER
} ElseIf($env:APPVEYOR_REPO_BRANCH -ne 'master' -and -not $env:APPVEYOR_REPO_BRANCH.StartsWith('release')) {
  $PreReleaseName = '-' + $env:APPVEYOR_REPO_BRANCH
} Else {
  $PreReleaseName = '' # This was previously: '-CI'
}

$totalVersion = "$ReleaseVersionNumber$PreReleaseName"

$PSScriptFilePath = (Get-Item $MyInvocation.MyCommand.Path).FullName
$ScriptDir = Split-Path -Path $PSScriptFilePath -Parent
$SolutionRoot = Split-Path -Path $ScriptDir -Parent

$csprojPath = Join-Path -Path $SolutionRoot -ChildPath "DeveImageOptimizerWPF\DeveImageOptimizerWPF.csproj"
$re = [regex]"(?<=<Version>).*(?=<\/Version>)"

Write-Host "Applying version $totalVersion to $csprojPath using regex $re"
 
$re.Replace([string]::Join("`n", (Get-Content -Path $csprojPath)), "$totalVersion", 1) | Set-Content -Path $csprojPath -Encoding UTF8