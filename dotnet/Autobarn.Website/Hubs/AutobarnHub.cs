using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Autobarn.Website.Hubs {
	public class AutobarnHub : Hub {
		public async Task ThisIsMagicStringNumberOne(string user, string message) {
			await Clients.All.SendAsync("ThisIsMagicStringNumberTwo", user, message);
		}
	}
}
