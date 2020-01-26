using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace RealtimePowerShellHTMLWindow
{
    public class OutputHub : Hub
    {
        public void Send(string message, string connID)
        {
            // Call the addNewMessageToPage method to update clients.
            // Clients.All.addNewMessageToPage(message);
            // Clients.Caller.addNewMessageToPage(message);
            Clients.Client(connID).addNewMessageToPage(message);
        }

        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;

            Groups.Add(Context.ConnectionId, name);

            return base.OnConnected();
        }
    }
}