using HB.NETF.Discord.NET.Toolkit.Services.ApplicationService;
using HB.NETF.Discord.NET.Toolkit.Services.EntityService;
using HB.NETF.Discord.NET.Toolkit.Services.EntityService.Holder;
using HB.NETF.Discord.NET.Toolkit.Services.TokenService;
using HB.NETF.Unity;
using Unity;

namespace HB.NETF.Discord.NET.Toolkit {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<IDiscordEntityService, DiscordRestEntityService>(nameof(DiscordRestEntityService))
                .RegisterType<IDiscordEntityService, DiscordSocketEntityService>(nameof(DiscordSocketEntityService))
                .RegisterType<IDiscordApplicationService, DiscordApplicationService>()
                .RegisterType<IDiscordTokenService, DiscordTokenService>()
                .RegisterSingleton<IServerCollectionHolder, ServerCollectionHolder>();
        }
    }
}
