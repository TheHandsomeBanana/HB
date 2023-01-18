using HB.DependencyInjection.Attributes;
using HB.Services.Logging.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.DependencyInjection {
    public class DIServiceCollection {
        private List<ServiceDescriptor> services = new List<ServiceDescriptor>();

        public DIServiceCollection RegisterSingleton<TService>(params object[]? optionalParams) {
            services.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Singleton, optionalParams));
            return this;
        }

        public DIServiceCollection RegisterSingleton<TService>(TService service) {
            if (service == null) throw new ArgumentNullException(nameof(service));

            services.Add(new ServiceDescriptor(service, ServiceLifetime.Singleton));
            return this;
        }

        public DIServiceCollection RegisterSingleton<TService, TImplementation>(params object[]? optionalParams) where TImplementation : TService {
            services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton, optionalParams));

            return this;
        }

        public DIServiceCollection RegisterTransient<TService>() {
            services.Add(new ServiceDescriptor(typeof(TService), ServiceLifetime.Transient));

            return this;
        }

        public DIServiceCollection RegisterTransient<TService, TImplementation>(params object[]? optionalParams) where TImplementation : TService {
            services.Add(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient, optionalParams));

            return this;
        }

        public void AddLogging() {

        }

        public DIContainer Resolve() {
            ResolveInternal();

            return new DIContainer(services.ToArray());
        }

        private void ResolveInternal() {       
            foreach(Type t in Assembly.GetCallingAssembly().GetTypes().AsSpan()) {

                foreach (PropertyInfo p in t.GetProperties())
                    SetProperty(p);

                foreach (FieldInfo f in t.GetFields())
                    SetField(f);
            }
        }

        private void SetProperty(PropertyInfo property) {
            SingletonDependencyAttribute? attr = property.GetCustomAttribute<SingletonDependencyAttribute>();
            if (attr == null)
                return;

            foreach(ServiceDescriptor sd in services) {

                if(sd.ServiceType.IsAssignableFrom(property.PropertyType) || sd.ImplementationType.IsAssignableFrom(property.PropertyType)) {
                }

            }
        }

        private void SetField(FieldInfo field) {

        }
    }
}
