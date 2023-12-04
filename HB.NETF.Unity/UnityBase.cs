using System;
using Unity;

namespace HB.NETF.Unity {
    public static class UnityBase {
        public static IUnityContainer UnityContainer { get; } = new UnityContainer();

        public static void Boot(params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(UnityContainer);
        }

        public static void Boot(IUnityContainer container, params IUnitySetup[] setups) {
            foreach (IUnitySetup setup in setups)
                setup.Build(container);
        }

        public static IUnityContainer GetChildContainer(string name) {
            if (UnityContainer.IsRegistered<IUnityContainer>(name))
                return UnityContainer.Resolve<IUnityContainer>(name);

            throw new Exception($"Child container {name} not registered.");
        }

        public static IUnityContainer CreateChildContainer(string name) {
            if (UnityContainer.IsRegistered<IUnityContainer>(name))
                return UnityContainer.Resolve<IUnityContainer>(name);

            IUnityContainer childContainer = UnityContainer.CreateChildContainer();
            UnityContainer.RegisterInstance(name, childContainer, InstanceLifetime.Singleton);
            return childContainer;
        }
    }
}
