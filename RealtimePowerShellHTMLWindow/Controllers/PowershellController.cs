using Microsoft.AspNet.SignalR;
using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Web.Mvc;

namespace RealtimePowerShellHTMLWindow.Controllers
{
    public class PowershellController : Controller
    {
        public void RunScript(PowerShell shell, bool varwidth)
        {
            // Sleep a few secs to allow enough time for Results window to open and establish connection to OutputHub
            // Without this, output may not show
            System.Threading.Thread.Sleep(3000);

            string hubGroup = User.Identity.Name;

            // Connect to OutputHub
            IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<OutputHub>();

            if (shell == null)
            {
                hub.Clients.Group(hubGroup).addNewMessageToPage("Shell empty - nothing to execute.");
                return;
            }

            string fontstr = "";
            if (varwidth != true)
            {
                fontstr = "face='monospace' size=3";
            }

            hub.Clients.Group(hubGroup).addNewMessageToPage("<b>Executing: </b>" + shell.Commands.Commands[0].ToString());
            string prevmsg = "";
            string msg = "";

            hub.Clients.Group(hubGroup).addNewMessageToPage("<br><b>BEGIN</b>");
            hub.Clients.Group(hubGroup).addNewMessageToPage("<br>_________________________________________________________________________");

            // Collect powershell OUTPUT and send to OutputHub
            var output = new PSDataCollection<PSObject>();

            output.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                msg = output[e.Index].ToString();

                if (msg != prevmsg)
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage("<br><span><font color=black " + fontstr + ">" + msg + "</font></span>");
                }
                else
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage(".");
                }
                prevmsg = msg;
                var psoutput = (PSDataCollection<PSObject>)sender;
                Collection<PSObject> results = psoutput.ReadAll();
            };

            prevmsg = "";
            // Collect powershell PROGRESS output and send to OutHub
            shell.Streams.Progress.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                msg = shell.Streams.Progress[e.Index].Activity.ToString();
                if (msg != prevmsg)
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage("<br><span><font color=green " + fontstr + ">" + msg + "</font></span>");
                }
                else
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage(".");
                }
                prevmsg = msg;
                var psprogress = (PSDataCollection<ProgressRecord>)sender;
                Collection<ProgressRecord> results = psprogress.ReadAll();
            };

            prevmsg = "";
            // Collect powershell WARNING output and send to OutHub
            shell.Streams.Warning.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                msg = shell.Streams.Warning[e.Index].ToString();
                if (msg != prevmsg)
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage("<br><span><font color=orange " + fontstr + "><b>***WARNING***:</b> " + msg + "</font></span>");
                }
                else
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage(".");
                }
                prevmsg = msg;
                var pswarning = (PSDataCollection<WarningRecord>)sender;
                Collection<WarningRecord> results = pswarning.ReadAll();
            };

            prevmsg = "";
            // Collect powershell ERROR output and send to OutHub
            shell.Streams.Error.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                msg = shell.Streams.Error[e.Index].ToString();
                if (msg != prevmsg)
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage("<br><span><font color=red " + fontstr + "><b>***ERROR***:</b> " + msg + "</font></span>");
                }
                else
                {
                    hub.Clients.Group(hubGroup).addNewMessageToPage(".");
                }
                prevmsg = msg;
                var pserror = (PSDataCollection<ErrorRecord>)sender;
                Collection<ErrorRecord> results = pserror.ReadAll();
            };

            // Execute powershell command
            IAsyncResult asyncResult = shell.BeginInvoke<PSObject, PSObject>(null, output);

            // Wait for powershell command to finish
            asyncResult.AsyncWaitHandle.WaitOne();

            // var results2 = shell.Invoke();
            // hub.Clients.Group(hubGroup).addNewMessageToPage(results2);

            hub.Clients.Group(hubGroup).addNewMessageToPage("<br>_________________________________________________________________________");
            hub.Clients.Group(hubGroup).addNewMessageToPage("<br><b>EXECUTION COMPLETE</b>. Check above results for any errors.");
            return;
        }
    }
}