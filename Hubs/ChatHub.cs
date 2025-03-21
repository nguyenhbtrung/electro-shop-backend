using electro_shop_backend.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace electro_shop_backend.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        // Lưu trạng thái cuộc trò chuyện: key = userId, value = adminId đã claim
        public static ConcurrentDictionary<string, string> ConversationLocks = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Người dùng gửi tin nhắn cho admin.
        /// Tin nhắn được gửi đến tất cả các admin trong group "Admins".
        /// </summary>
        /// <param name="message"></param>
        public async Task SendMessageToAdmins(string message)
        {
            var userId = Context.UserIdentifier;
            var userName = Context.User?.Identity?.Name;

            // Gửi tin nhắn tới group "Admins" để tất cả admin đều thông báo
            await Clients.Group("Admins").SendAsync("ReceiveUserMessage", userId, message, userName);

            // Nếu muốn, gửi lại cho chính user để cập nhật UI
            //await Clients.User(userId).SendAsync("ReceiveMessage", $"Bạn: {message}");
        }

        /// <summary>
        /// Admin gọi để claim (đảm nhận) cuộc trò chuyện của một user.
        /// Nếu cuộc trò chuyện chưa được claim, admin claim thành công.
        /// Nếu đã có admin claim, thông báo cho caller.
        /// </summary>
        /// <param name="userId">ID của user cần xử lý</param>
        public async Task ClaimConversation(string userId)
        {
            var adminId = Context.UserIdentifier;
            bool claimed = ConversationLocks.TryAdd(userId, adminId);
            if (claimed)
            {
                // Thông báo tới tất cả admin biết rằng cuộc trò chuyện của user đã được claim bởi adminId
                await Clients.Group("Admins").SendAsync("ConversationClaimed", userId, adminId);
                // Thông báo cho chính user rằng admin đã đảm nhận
                await Clients.User(userId).SendAsync("ConversationClaimed", adminId);
                await Clients.Caller.SendAsync("ClaimStatus", "Bạn đã claim thành công cuộc trò chuyện này.");
            }
            else
            {
                string currentAdmin = ConversationLocks[userId];
                if (currentAdmin == adminId)
                {
                    await Clients.Caller.SendAsync("ClaimStatus", "Bạn đã claim cuộc trò chuyện này.");
                }
                else
                {
                    await Clients.Caller.SendAsync("ClaimStatus", $"Cuộc trò chuyện đã được claim bởi quản trị viên khác ({currentAdmin}).");
                }
            }
        }

        /// <summary>
        /// Admin sau khi đã claim gọi phương thức này để gửi tin nhắn trả lời cho user.
        /// Chỉ admin đã claim mới được gửi.
        /// </summary>
        /// <param name="userId">ID của user nhận tin</param>
        /// <param name="message">Nội dung tin nhắn</param>
        public async Task SendAdminMessage(string userId, string message)
        {
            var adminId = Context.UserIdentifier;
            if (ConversationLocks.TryGetValue(userId, out string assignedAdmin))
            {
                if (assignedAdmin == adminId)
                {
                    // Gửi tin nhắn cho user đã claim
                    await Clients.User(userId).SendAsync("ReceiveAdminMessage", message);
                    // Phản hồi lại admin (update UI)
                    //await Clients.Caller.SendAsync("ReceiveMessage", $"Bạn: {message}");
                }
                else
                {
                    // Nếu admin này chưa được claim thì không được gửi
                    await Clients.Caller.SendAsync("Error", "Cuộc trò chuyện đã được claim bởi quản trị viên khác. Không được phép gửi tin.");
                }
            }
            else
            {
                // Chưa có admin claim cho cuộc trò chuyện này
                await Clients.Caller.SendAsync("Error", "Chưa có admin nắm quyền cho cuộc trò chuyện này. Vui lòng claim trước.");
            }
        }

        /// <summary>
        /// Admin giải phóng cuộc trò chuyện khi đã hoàn tất (hoặc chuyển giao).
        /// </summary>
        /// <param name="userId">ID của user</param>
        public async Task ReleaseConversation(string userId)
        {
            var adminId = Context.UserIdentifier;
            if (ConversationLocks.TryGetValue(userId, out string currentAdmin) && currentAdmin == adminId)
            {
                ConversationLocks.TryRemove(userId, out _);
                await Clients.Group("Admins").SendAsync("ConversationReleased", userId, adminId);
                await Clients.User(userId).SendAsync("ConversationReleased", adminId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Không thể giải phóng cuộc trò chuyện này.");
            }
        }

        /// <summary>
        /// Khi kết nối, nếu user thuộc role "Admin", thêm họ vào group "Admins"
        /// Điều này giúp việc truyền tin cho tất cả admin trở nên dễ dàng.
        /// </summary>
        public override async Task OnConnectedAsync()
        {
            if (Context.User.IsInRole("Admin"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.UserIdentifier;

            // Nếu user đang đóng vai trò Admin
            if (Context.User.IsInRole("Admin"))
            {
                // Tìm tất cả các cuộc trò chuyện mà admin này đã claim (key = userId, value = adminId)
                var lockedConversations = ConversationLocks
                    .Where(lockItem => lockItem.Value == userId)
                    .Select(lockItem => lockItem.Key)
                    .ToList();

                foreach (var conversationUserId in lockedConversations)
                {
                    // Loại bỏ lock của conversation
                    ConversationLocks.TryRemove(conversationUserId, out _);
                    // Thông báo cho group "Admins" và cho user tương ứng
                    await Clients.Group("Admins").SendAsync("ConversationReleased", conversationUserId, userId);
                    await Clients.User(conversationUserId).SendAsync("ConversationReleased", userId);
                }
            }
            //else
            //{
            //    // Nếu người dùng ngắt kết nối và có lock liên quan, loại bỏ nó
            //    if (ConversationLocks.TryRemove(userId, out string adminId))
            //    {
            //        await Clients.Group("Admins").SendAsync("ConversationReleased", userId, adminId);
            //    }
            //}

            await base.OnDisconnectedAsync(exception);
        }


    }
}
