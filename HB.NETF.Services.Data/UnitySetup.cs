using HB.NETF.Services.Data.Handler;
using HB.NETF.Services.Data.Handler.Async;
using HB.NETF.Services.Data.Holder;
using HB.NETF.Services.Data.Identifier;
using HB.NETF.Unity;
using Unity;

namespace HB.NETF.Services.Data {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<IStreamHandler, StreamHandler>()
                .RegisterType<IAsyncStreamHandler, AsyncStreamHandler>()
                .RegisterType<IDataHolder, DataHolder>()
                .RegisterType<IIdentifierFactory, IdentifierFactory>();
        }
    }
}
