using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject.Components;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Advice;
using Ninject.Extensions.Interception.Planning.Directives;
using Ninject.Extensions.Interception.Registry;
using Ninject;
using Ninject.Planning;
using Ninject.Planning.Strategies;

namespace CIS.UI.Bootstraps.InversionOfControl.Ninject.Interceptors
{
    public class CustomPlanningStrategy<TAttribute, TInterceptor> : NinjectComponent, IPlanningStrategy
        where TAttribute : Attribute
        where TInterceptor : IInterceptor
    {
        private readonly IAdviceFactory adviceFactory;
        private readonly IAdviceRegistry adviceRegistry;

        public CustomPlanningStrategy(IAdviceFactory adviceFactory, IAdviceRegistry adviceRegistry)
        {
            this.adviceFactory = adviceFactory;
            this.adviceRegistry = adviceRegistry;
        }

        public void Execute(IPlan plan)
        {
            var methods = GetCandidateMethods(plan.Type);
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(TAttribute), true) as TAttribute[];
                if (attributes.Length == 0)
                    continue;

                var advice = adviceFactory.Create(method);
                advice.Callback = request => request.Kernel.Get<TInterceptor>();
                adviceRegistry.Register(advice);

                if (plan.Has<ProxyDirective>() != true)
                    plan.Add(new ProxyDirective());
            }
        }

        private static IEnumerable<MethodInfo> GetCandidateMethods(Type type)
        {
            var methods = type.GetMethods(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance
            );

            return methods.Where(ShouldIntercept);
        }

        private static bool ShouldIntercept(MethodInfo methodInfo)
        {
            return methodInfo.DeclaringType != typeof(object) && !methodInfo.IsPrivate && !methodInfo.IsFinal;
        }
    }
}