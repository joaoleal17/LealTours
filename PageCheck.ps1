<#
  PageCheck.ps1 (v2.1 - compatível com PowerShell 5.x)
  Varre um projeto .NET MAUI/WPF/Xamarin para verificar:
  - Ficheiros XAML vs x:Class vs partial class
  - Rotas registadas vs navegação (GoToAsync)
  - Conflitos BookingPage vs BookingsPage
#>

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

Write-Host "=== Verificação de Páginas / Rotas / DI ===`n"

function Safe-ReadText {
    param([Parameter(Mandatory=$true)][string]$Path)
    try {
        if (-not (Test-Path -LiteralPath $Path)) { return "" }
        $txt = Get-Content -Raw -LiteralPath $Path -ErrorAction Stop
        if ($null -eq $txt) { return "" }
        return [string]$txt
    } catch {
        return ""
    }
}

# 1) Mapear XAML -> x:Class
$xamlInfos = @()

Get-ChildItem -Recurse -Include *.xaml -File | ForEach-Object {
    $path = $_.FullName
    $content = Safe-ReadText -Path $path
    if ([string]::IsNullOrWhiteSpace($content)) { return }

    $m = [regex]::Match($content, 'x:Class\s*=\s*"([^"]+)"')
    if ($m.Success) {
        $xClass = $m.Groups[1].Value
        $fileName = [System.IO.Path]::GetFileNameWithoutExtension($path)
        $xamlInfos += [pscustomobject]@{
            File        = $path
            FileName    = $fileName
            XClass      = $xClass
            ClassShort  = ($xClass.Split('.') | Select-Object -Last 1)
        }
    }
}

# 2) Mapear code-behind -> partial class
$csInfos = @()
Get-ChildItem -Recurse -Include *.cs -File |
    Where-Object { $_.Name -notmatch '\.(g|g\.i|designer)\.cs$' } |
    ForEach-Object {
        $path = $_.FullName
        $content = Safe-ReadText -Path $path
        if ([string]::IsNullOrWhiteSpace($content)) { return }

        $ns = $null
        $nsMatch = [regex]::Match($content, '^\s*namespace\s+([^\s;{]+)', 'Multiline')
        if ($nsMatch.Success) { $ns = $nsMatch.Groups[1].Value }

        foreach ($m in [regex]::Matches($content, 'partial\s+class\s+([A-Za-z0-9_]+Page)\s*:\s*[A-Za-z0-9_\.]+')) {
            $className = $m.Groups[1].Value
            $csInfos += [pscustomobject]@{
                File       = $path
                Namespace  = $ns
                ClassName  = $className
                FullName   = $(if ($ns) { "$ns.$className" } else { $className })
            }
        }
    }

# 3) Rotas registadas e navegação
$routes = @()
$navs   = @()

Get-ChildItem -Recurse -Include *.cs -File | ForEach-Object {
    $path = $_.FullName
    $content = Safe-ReadText -Path $path
    if ([string]::IsNullOrWhiteSpace($content)) { return }

    foreach ($m in [regex]::Matches($content, 'Routing\.RegisterRoute\(([^)]+)\)')) {
        $routes += [pscustomobject]@{
            File = $path
            Code = $m.Value.Trim()
        }
    }
    foreach ($m in [regex]::Matches($content, 'GoToAsync\(([^)]+)\)')) {
        $navs += [pscustomobject]@{
            File = $path
            Code = $m.Value.Trim()
        }
    }
}

# 4) Cruzar XAML vs Code-behind
Write-Host "`n--- XAML vs partial class ---"
if ($xamlInfos.Count -eq 0) {
    Write-Host "(nenhum XAML encontrado — estás a correr na raiz da solução/projeto?)"
} else {
    foreach ($x in $xamlInfos | Sort-Object File) {
        $match = $csInfos | Where-Object { $_.ClassName -eq $x.ClassShort }
        $status = if ($match) { "OK" } else { "❌ sem partial class correspondente" }

        if ($x.FileName -ne $x.ClassShort) { $status += " | ⚠️ FileName≠ClassShort ($($x.FileName) vs $($x.ClassShort))" }

        "{0}`n  x:Class: {1}`n  File:    {2}`n  Status:  {3}`n" -f $x.ClassShort, $x.XClass, $x.File, $status
    }
}

# 5) Procurar conflitos BookingPage vs BookingsPage
Write-Host "`n--- Possíveis conflitos Booking(s)Page ---"
$hits = Get-ChildItem -Recurse -Include *.cs,*.xaml -File |
    Select-String -Pattern 'BookingPage','BookingsPage' -List -ErrorAction SilentlyContinue
if ($hits) {
    $hits | ForEach-Object {
        $lineText = ""
        if ($_.Line) { $lineText = $_.Line.Trim() }
        "• {0}:{1}  {2}" -f $_.Path, $_.LineNumber, $lineText
    }
} else {
    Write-Host "Sem ocorrências de BookingPage/BookingsPage encontradas."
}

# 6) Rotas e navegação
Write-Host "`n--- Rotas registadas ---"
if ($routes.Count -eq 0) { Write-Host "(nenhuma encontrada)" }
$routes | Sort-Object File | ForEach-Object { "• {0} -> {1}" -f $_.File, $_.Code }

Write-Host "`n--- Chamadas de navegação (GoToAsync) ---"
if ($navs.Count -eq 0) { Write-Host "(nenhuma encontrada)" }
$navs | Sort-Object File | ForEach-Object { "• {0} -> {1}" -f $_.File, $_.Code }

Write-Host "`n=== Fim ==="