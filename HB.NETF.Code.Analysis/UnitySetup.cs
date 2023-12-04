using HB.NETF.Code.Analysis.Analyser;
using HB.NETF.Code.Analysis.Factory;
using HB.NETF.Code.Analysis.Interface;
using HB.NETF.Unity;
using Unity;

namespace HB.NETF.Code.Analysis {
    public class UnitySetup : IUnitySetup {
        public void Build(IUnityContainer container) {
            container.RegisterType<IAnalyserFactory, AnalyserFactory>();
            IUnityContainer childContainer = container.CreateChildContainer();
            childContainer.RegisterType<ITypeAnalyser, TypeAnalyser>()
                .RegisterType<IVariableAnalyser, VariableAnalyser>()
                .RegisterType<IIdentifierAnalyser, IdentifierAnalyser>();

            container.RegisterInstance<IUnityContainer>("HB.NETF.Code.Analysis", childContainer);
        }
    }
}
