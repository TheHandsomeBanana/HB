using HB.NETF.Discord.NET.Toolkit.EntityService.Models;
using HB.NETF.Discord.NET.Toolkit.Models;
using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using HB.NETF.Services.Data.Handler.Manipulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.NETF.Discord.NET.Toolkit.EntityService.Merged {
    public interface IMergedDiscordEntityService : IStreamManipulator, IDisposable {
        void Init(params string[] tokens);
        void Init(params TokenModel[] tokens);
        Task SaveMerged(string mergedPath);
        Task<bool> LoadMerged(string mergedPath);
        DiscordServerCollection ServerCollection { get; }
    }
}
