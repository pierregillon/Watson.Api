using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Watson.Infrastructure
{
    public class InMemoryDatabase
    {
        private IDictionary<Type, IList> _tables = new ConcurrentDictionary<Type, IList>();

        public IList<T> Table<T>()
        {
            lock (_tables) {
                if (_tables.ContainsKey(typeof(T)) == false) {
                    _tables.Add(typeof(T), CreateList<T>());
                }
                return (IList<T>)_tables[typeof(T)];
            }
        }

        private IList CreateList<T>()
        {
            return (IList)typeof(List<>)
                .MakeGenericType(typeof(T))
                .GetConstructors()[0]
                .Invoke(new object[] { });
        }
    }
}