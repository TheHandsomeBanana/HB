using Unity;

namespace HB.DependencyInjection.Unity;
public interface IUnitySetup {
    void Build(IUnityContainer container);
}
