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