using System.Management.Automation;
using System.Web.Mvc;

namespace RealtimePowerShellHTMLWindow.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string target)
        {
            // If user provides path, ensure all "\" are converted to "\\"
            target = target.Replace("\\", "\\\\");

            // Prepend with "~\Scripts\Powershell\" - required location of PowerShell scripts
            target = "~\\Scripts\\PowerShell\\" + target;

            string pscommand = Server.MapPath(target);
            PowerShell shell = PowerShell.Create();

            // Setup powershell command and execute
            shell.AddCommand(pscommand);
            //    .AddParameter("target", target); //Add PowerShell script parameters here if you need

            var powershellcontroller = DependencyResolver.Current.GetService<PowershellController>();
            powershellcontroller.ControllerContext = new ControllerContext(this.Request.RequestContext, powershellcontroller);

            powershellcontroller.RunScript(shell, true);

            return View();
        }
        public ActionResult Results()
        {
            return View();
        }
    }
}