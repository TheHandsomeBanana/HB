﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HB.Utilities.DependencyInjection {
    internal class ServiceDescriptor {
        public Type ServiceType { get; }
        public Type ImplementationType { get; set; }
        public object? Implementation { get; internal set; }
        public object[]? OptionalParams { get; }
        public ServiceLifetime LifeTime { get; }

        public ServiceDescriptor(Type serviceType, ServiceLifetime lifeTime, object[]? optionalParams) {
            ServiceType = serviceType;
            LifeTime = lifeTime;
            OptionalParams = optionalParams;
        }

        public ServiceDescriptor(object implementation, ServiceLifetime lifeTime) {
            ServiceType = implementation.GetType();
            Implementation = implementation;
            LifeTime = lifeTime;
        }

        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifeTime, object[]? optionalParams) {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            LifeTime = lifeTime;
            OptionalParams = optionalParams;
        }
    }
}
