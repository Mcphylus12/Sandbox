using PermissionsModels;
using System;
using System.Collections.Generic;

namespace Permissions.Tests.Helpers
{
    public static class ObjectExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (T item in collection)
            {
                action(item);
            }
        
        
        }

        public static bool IdMatches(this IHasId thiss, IHasId other) => other != null && thiss != null && other.Id == thiss.Id;
    }
}
