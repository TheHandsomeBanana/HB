using Unity;

namespace HB.DependencyInjection.Unity {
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

        public static IUnityContainer? GetChildContainer(string name) {
            if (UnityContainer.IsRegistered<IUnityContainer>(name))
                return UnityContainer.Resolve<IUnityContainer>(name);

            return null;
        }

        public static IUnityContainer CreateOrGetChildContainer(string name) {
            if (UnityContainer.IsRegistered<IUnityContainer>(name))
                return UnityContainer.Resolve<IUnityContainer>(name);

            IUnityContainer childContainer = UnityContainer.CreateChildContainer();
            UnityContainer.RegisterInstance(name, childContainer, InstanceLifetime.Singleton);
            return childContainer;
        }
    }
}
