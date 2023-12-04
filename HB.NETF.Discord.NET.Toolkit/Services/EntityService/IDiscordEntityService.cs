using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Services.Data.Handler.Manipulator;
using System;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService {
    public delegate void ConnectionTimeout();
    public delegate void ConnectionReady();
    public interface IDiscordEntityService : IStreamManipulator, IDisposable, IAsyncDisposable {
        event ConnectionTimeout OnTimeout;
        int Timeout { get; set; }
        bool Ready { get; }
        Task Connect(string token);

        Task<DiscordServerCollection> LoadEntities();
        Task Disconnect();

        Task SaveToFile(string fileName, DiscordServerCollection serverCollection);
        Task<DiscordServerCollection> ReadFromFile(string fileName);
    }
}
