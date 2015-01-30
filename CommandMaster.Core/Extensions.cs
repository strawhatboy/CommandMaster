using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandMaster.Core
{
    public static class Extensions
    {
        public static void Each<T>(this IEnumerable<T> elements, Action<T> action)
        {
            foreach (var element in elements)
            {
                action(element);
            }
        }
    }
}
