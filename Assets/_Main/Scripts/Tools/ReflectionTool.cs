using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Tools
{
    public static class ReflectionTool
    {
        private static Dictionary<string, Type> _cache;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void BuildCache()
        {
            _cache = new Dictionary<string, Type>(2048);
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var name = assembly.FullName;
                if (name.StartsWith("System") ||
                    name.StartsWith("Microsoft") ||
                    name.StartsWith("mscorlib") ||
                    name.StartsWith("netstandard") ||
                    name.StartsWith("Mono.")) 
                { continue; }
                
                foreach (var type in assembly.GetTypes())
                { if (type.FullName != null) _cache.TryAdd(type.Name, type); }
            }
        }
        
        public static Type GetTypeByName(string fullName) => _cache.GetValueOrDefault(fullName);
        
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