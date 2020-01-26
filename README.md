# Real-time PowerShell HTML Window
Asynchronous PowerShell script output to web page

This is a working ASP.NET demonstration of how to asynchronously (in real-time) execute PowerShell scripts, capturing and writing the Output, Progress, Warning, and Error streams to an HTML results window.

## Prerequisites

* IIS must have Windows authentication enabled, and anonymous authenticated disabled. This is because SignalR is used to provide live feedback of the running script - as such, a SignalR group is created using the username associated with each session.

* PowerShell scripts must be non-interactive; there's no means to provide input back to powershell scripts once they're running.

* Output must be written using Write-Output, Write-Progress, Write-Warning, or Write-Error only. Output written with Write-Host cannot be captured and hence won't display in the results window.

* Ensure the PowerShell Execution Policy has been sufficiently opened to allow your scripts to run. If in doubt and at own risk, as Administrator run:
    - Set-ExecutionPolicy Unrestricted -Scope LocalMachine
    - Set-ExecutionPolicy Unrestricted -Scope CurrentUser
    - Set-ExecutionPolicy Unrestricted -Scope Process
    
* Place your PowerShell scripts in **~/Scripts/PowerShell**. ASP.NET requires the path to be relative to the project, hence this location. You'll find a cute test.ps1 file in there.

## Running the tests

Specify **test.ps1** as the script to execute. It provides output to each of the PowerShell streams over 5 iterations, with a sleep-start 1s after each iteration.


## Authors

* **Ashley Strahle** - [AshStrahle](https://github.com/AshStrahle)
