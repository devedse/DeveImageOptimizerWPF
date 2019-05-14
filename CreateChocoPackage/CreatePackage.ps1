$ErrorActionPreference = "Stop"

$fileToPackage = "Scripts\Output\DeveImageOptimizerWPF.exe"

Function Remove-Comments($Path)
{

    Get-ChildItem $Path -ErrorAction Stop | Out-Null
    $ScriptBlockString = [IO.File]::ReadAllText((Resolve-Path $Path))
    $ScriptBlock = [ScriptBlock]::Create($ScriptBlockString)


    # Tokenize the scriptblock and return all tokens except for comments
    $Tokens = [System.Management.Automation.PSParser]::Tokenize($ScriptBlock, [Ref] $Null) | Where-Object { $_.Type -ne 'Comment' }

    $StringBuilder = New-Object Text.StringBuilder

    # The majority of the remaining code comes from Lee Holmes' Show-ColorizedContent script.
    $CurrentColumn = 1
    $NewlineCount = 0
    foreach($CurrentToken in $Tokens)
    {
        # Now output the token
        if(($CurrentToken.Type -eq 'NewLine') -or ($CurrentToken.Type -eq 'LineContinuation'))
        {
            $CurrentColumn = 1
            # Only insert a single newline. Sequential newlines are ignored in order to save space.
            if ($NewlineCount -eq 0)
            {
                $StringBuilder.AppendLine() | Out-Null
            }
            $NewlineCount++
        }
        else
        {
            $NewlineCount = 0

            # Do any indenting
            if($CurrentColumn -lt $CurrentToken.StartColumn)
            {
                # Insert a single space in between tokens on the same line. Extraneous whiltespace is ignored.
                if ($CurrentColumn -ne 1)
                {
                    $StringBuilder.Append(' ') | Out-Null
                }
            }

            # See where the token ends
            $CurrentTokenEnd = $CurrentToken.Start + $CurrentToken.Length - 1

            # Handle the line numbering for multi-line strings
            if(($CurrentToken.Type -eq 'String') -and ($CurrentToken.EndLine -gt $CurrentToken.StartLine))
            {
                $LineCounter = $CurrentToken.StartLine
                $StringLines = $(-join $ScriptBlockString[$CurrentToken.Start..$CurrentTokenEnd] -split '`r`n')

                foreach($StringLine in $StringLines)
                {
                    $StringBuilder.Append($StringLine) | Out-Null
                    $LineCounter++
                }
            }
            # Write out a regular token
            else
            {
                $StringBuilder.Append((-join $ScriptBlockString[$CurrentToken.Start..$CurrentTokenEnd])) | Out-Null
            }

            # Update our position in the column
            $CurrentColumn = $CurrentToken.EndColumn
        }
    }

    $StringBuilder.ToString() | Set-Content -Path $Path
    #Write-Output ([ScriptBlock]::Create($StringBuilder.ToString()))
}

$invocation = (Get-Variable MyInvocation).Value
$directorypath = Split-Path $invocation.MyCommand.Path
$SolutionRoot = Split-Path -Path $directorypath -Parent

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
$chocoinstallpsfiletemplatepath = Join-path $templateDir "chocolateyinstall.ps1"
$chocouninstallpsfiletemplatepath = Join-path $templateDir "chocolateyuninstall.ps1"
$chocobeforemodifypsfiletemplatepath = Join-path $templateDir "chocolateybeforemodify.ps1"

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
Copy-Item $chocouninstallpsfiletemplatepath $chocouninstallpsfilepath
Copy-Item $chocobeforemodifypsfiletemplatepath $chocobeforemodifypsfilepath

Write-Host "Removing comments..."
Remove-Comments($chocoinstallpsfilepath)
Remove-Comments($chocouninstallpsfilepath)
Remove-Comments($chocobeforemodifypsfilepath)


$ReleaseVersionNumberFull = (Get-Item $fullPathFileToPackage).VersionInfo.FileVersion
#$checksum = checksum -t sha256 -f $fullPathFileToPackage
$checksum = checksum -t sha256 -f $destinationFilePath7z

$nuspecFile = (Get-ChildItem "$($directorypath)\*" -include *.nuspec).FullName


$re = [regex]"(?<=<version>).*(?=<\/version>)"
$reChecksum = [regex]"{checksumtoreplace}"

Write-Host "Setting version $ReleaseVersionNumberFull in $nuspecFile by using Regex: $re"
 
$re.Replace([string]::Join("`n", (Get-Content -Path $nuspecFile)), "$ReleaseVersionNumberFull", 1) | Set-Content -Path $nuspecFile -Encoding UTF8

Write-Host "Setting checksum $checksum in $chocoinstallpsfilepath by using Regex: $reChecksum"

$reChecksum.Replace([string]::Join("`n", (Get-Content -Path $chocoinstallpsfilepath)), "$checksum", 1) | Set-Content -Path $chocoinstallpsfilepath -Encoding UTF8

Push-Location -Path $directorypath
choco pack
Pop-Location