using System;
using System.Linq;
using System.Reflection;

namespace Watson.Infrastructure
{
    public class ReflectionTypeLocator : ITypeLocator
    {
        private static readonly Type[] _types = Assembly.GetExecutingAssembly().GetTypes();

        public Type Find(string typeName)
        {
            return _types.SingleOrDefault(x => x.Name == typeName);
        }
    }
}