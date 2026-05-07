using System;
using System.Linq;
using System.Reflection;

namespace Tools
{
    public static class ReflectionTool
    {
        public static Type[] GetSubclasses<T>()
        {
            var baseType = typeof(T);
            var assembly = Assembly.GetAssembly(baseType);
            var types = assembly.GetTypes();
            var subs = types.Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract).ToArray();
            
            return subs;
        }
    }
}