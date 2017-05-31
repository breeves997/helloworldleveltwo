using Microsoft.ServiceFabric.Services.Remoting;
using System.Threading.Tasks;

namespace MessagingService.Contracts
{
    public interface IMessagingService : IService
    {
        Task<bool> SendMessage(string recipientName, Message message);
    }
}
