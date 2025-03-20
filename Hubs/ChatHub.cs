using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace electro_shop_backend.Hubs
{
    public class ChatHub : Hub
    {


        // Phương thức này nhận 'user' và 'message' từ client
        // Sau đó gửi tin nhắn đến tất cả các client đang kết nối
        //[Authorize]
        public async Task SendMessage(string user, string message)
        {
             if (Context.User.IsInRole("Admin"))
                await Clients.All.SendAsync("ReceiveMessage", user, "admin");
            else
                await Clients.All.SendAsync("ReceiveMessage", user, "user");

        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Clients.All.SendAsync("ReceiveMessage", "admin", "admin join");
            }
            else
                await Clients.All.SendAsync("ReceiveMessage", "user", "user join");

            await base.OnConnectedAsync();
        }
    }
}
