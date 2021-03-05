using System;
using System.Collections.Generic;
using System.Linq;

namespace AI_LAB_2.Extensions
{
    public static class EnumerableExtensions 
    {
        public static T MinBy<T>(this IEnumerable<T> data, Func<T, IComparable> selector)
        {
            int count = data.Count();

            if (count == 0)
                return default;

            int besti = 0;
            IComparable min = selector(data.ElementAt(0));

            for(int i = 1; i < count; ++i)
            {
                IComparable value = selector(data.ElementAt(i));
                if (value.CompareTo(min) < 0)
                {
                    besti = i;
                    min = value;
                }
            }

            return data.ElementAt(besti);
        }
    }
}
