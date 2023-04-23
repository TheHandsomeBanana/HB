using HB.DependencyInjection.Custom.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HB.DependencyInjection.Custom
{
    public class DIContainer : IDisposable
    {
        private ServiceDescriptor[] services;
        internal DIContainer(ServiceDescriptor[] serviceDescriptors)
        {
            services = serviceDescriptors;
        }

        public object? GetService(Type serviceType)
        {
            ServiceDescriptor descriptor = services.SingleOrDefault(s => s.ServiceType == serviceType)
                ?? throw new DependencyException($"Service with type of {serviceType.FullName} is not registered");

            if (descriptor.Implementation != null)
                return descriptor.Implementation;

            Type definedType = descriptor.ImplementationType ?? descriptor.ServiceType;
            if (definedType.IsAbstract || definedType.IsInterface)
                throw new DependencyException("Cannot instantiate abstract classes or interfaces");

            ConstructorInfo ctorInfo = definedType.GetConstructors().First();
            List<object> parameters = new List<object>();

            int optionalParamsCounter = 0;
            foreach (ParameterInfo p in ctorInfo.GetParameters())
            {
                if (services.Any(e => e.ServiceType == p.ParameterType))
                {
                    object temp = GetService(p.ParameterType) ?? throw new DependencyException($"Could not resolve parameter {p.Name}. Cannot get service {serviceType.FullName}");
                    parameters.Add(temp);
                }
                else
                {
                    if (descriptor.OptionalParams == null)
                        throw new DependencyException($"No service registered for {p.ParameterType}. No optional parameter for {p.Name} of type {p.ParameterType} provided.");

                    if (p.ParameterType != descriptor.OptionalParams[optionalParamsCounter].GetType())
                        throw new DependencyException($"Type mismatch in {nameof(descriptor.OptionalParams)}. {p.ParameterType.FullName} != {descriptor.OptionalParams.GetType().FullName}.");

                    parameters.Add(descriptor.OptionalParams[optionalParamsCounter]);
                    optionalParamsCounter++;
                }
            }

            object? implementation = Activator.CreateInstance(definedType, parameters);

            if (descriptor.LifeTime == ServiceLifetime.Singleton)
                descriptor.Implementation = implementation;

            return descriptor.Implementation;
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        public void Dispose()
        {
            foreach (ServiceDescriptor sd in services)
                sd.Dispose();
        }
    }
}
