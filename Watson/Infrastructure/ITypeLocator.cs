using System;

namespace Watson.Infrastructure
{
    public interface ITypeLocator
    {
        Type Find(string typeName);
    }
}