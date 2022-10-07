using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetBookGamingMobile.Extensions;

public static class ListExtensions
{
    public static void AddRange<T>(
    this ObservableCollection<T> collection, IEnumerable<T> newItems, bool clearFirst = false)
    {
        if (clearFirst)
            collection.Clear();

        newItems.ForEach(newItem => collection.Add(newItem));
    }

    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
    {
        foreach (var item in enumeration)
        {
            action(item);
        }
    }
}
