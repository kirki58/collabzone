using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace collabzone.Hubs;

public sealed class ProjectChatHub : Hub
{
    public async Task JoinProject(Guid guid){
        await Groups.AddToGroupAsync(Context.ConnectionId, guid.ToString());
    }
    
    public async Task LeaveProject(Guid guid){
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, guid.ToString());
    }

    public async Task SendMessage(Guid guid,int id, string message){
        await Clients.Group(guid.ToString()).SendAsync("ReceiveMessage", id ,message);
    }

    public async Task RefreshGuid(Guid oldGuid,Guid newGuid){
        await Clients.Group(oldGuid.ToString()).SendAsync("refreshGuid", newGuid);
    }
}
