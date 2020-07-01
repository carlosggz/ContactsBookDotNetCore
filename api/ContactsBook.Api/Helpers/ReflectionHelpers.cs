using ContactsBook.Api.EventsSubscribers;
using ContactsBook.Common.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ContactsBook.Api.Helpers
{
    public static class ReflectionHelpers
    {
        static public ReadOnlyDictionary<Type, Type> EventsSubscribers { get; private set; }
        static ReflectionHelpers()
        {
            var dict = new Dictionary<Type, Type>();

            foreach (var t in GetAllTypesImplementingOpenGenericType(typeof(IDomainEventSubscriber<>), Assembly.GetAssembly(typeof(ReflectionHelpers))))
                if (t.GetCustomAttribute(typeof(EventSubscriberAttribute)) is EventSubscriberAttribute attr)
                    dict.Add(t, attr.DomainEventType);

            EventsSubscribers = new ReadOnlyDictionary<Type, Type>(dict);
        }

        private static IEnumerable<Type> GetAllTypesImplementingOpenGenericType(Type myGenericType, Assembly assembly)
        {
            return from x in assembly.GetTypes()
                   from z in x.GetInterfaces()
                   let y = x.BaseType
                   where y != null && y.IsGenericType && myGenericType.IsAssignableFrom(y.GetGenericTypeDefinition()) ||
                         z.IsGenericType && myGenericType.IsAssignableFrom(z.GetGenericTypeDefinition())
                   select x;
        }
    }
}
