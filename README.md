# Real-time PowerShell HTML Window
Asynchronous PowerShell script output to web page

Useful for automation and user accessibility, this is a working ASP.NET demonstration of how to asynchronously (in real-time) execute PowerShell scripts, capturing and writing the Output, Progress, Warning, and Error streams to an HTML results window.

![Results1](https://github.com/ashstrahle/RealtimePowerShellHTMLWindow/blob/master/Images/Results1.png)

## Prerequisites

* IIS must have Windows authentication enabled, and anonymous authenticated disabled. This is because SignalR is used to provide live feedback of the running script - as such, a SignalR group is created using the username associated with each session. If set correctly, you should see your username in the top right of the initial form.

* PowerShell scripts must be non-interactive; there's no means to provide input back to powershell scripts once they're running.

* Output must be written using Write-Output, Write-Progress, Write-Warning, or Write-Error only. Output written with Write-Host cannot be captured and hence won't display in the results window.

* Ensure the PowerShell Execution Policy has been sufficiently opened to allow your scripts to run. If in doubt and at own risk, as Administrator run:
    - Set-ExecutionPolicy Unrestricted -Scope LocalMachine
    - Set-ExecutionPolicy Unrestricted -Scope CurrentUser
    - Set-ExecutionPolicy Unrestricted -Scope Process
    
* Place your PowerShell scripts in **~/Scripts/PowerShell**. ASP.NET requires the path to be relative to the project, hence this location. You'll find a cute test.ps1 file in there.

## Running the tests

**test.ps1** included in this project, contains the following code:
```powershell
for ($i=1; $i -le 5; $i++) {
    Write-Progress "Loop $i - progress output (Write-Progress)"
    Write-Output "Normal output text (Write-Output)"
    Write-Warning "Here's some warning text (Write-Warning)"
    Write-Error "Oh no, here's some error text (Write-Error)"
    Start-Sleep -s 1
}
```

It provides output to each of the PowerShell streams over 5 iterations, with a start-sleep 1s after each iteration, and produces the following:

![Image 1](https://github.com/ashstrahle/RealtimePowerShellHTMLWindow/blob/master/Images/Run%20PowerShell%20Script.png)

![Results1](https://github.com/ashstrahle/RealtimePowerShellHTMLWindow/blob/master/Images/Results1.png)

![Results2](https://github.com/ashstrahle/RealtimePowerShellHTMLWindow/blob/master/Images/Results2.png)


## Author

* **Ashley Strahle** - [AshStrahle](https://github.com/AshStrahle)
