using HB.NETF.Unity;
using Unity;

namespace HB.NETF.Services.Serialization {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<ISerializerService, SerializerService>();
        }
    }
}
