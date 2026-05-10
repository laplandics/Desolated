using System;
using System.Collections;
using System.Collections.Generic;
using Service;

namespace Service { public abstract class ServiceBase { } }

public interface IOnProjectBeginLoadService { public IEnumerator OnProjectBeginLoad(); }
public interface IOnProjectEndLoadService { public IEnumerator OnProjectEndLoad(); }
public interface IOnSceneBeginLoadService { public IEnumerator OnSceneBeginLoad(); }
public interface IOnSceneEndLoadService { public IEnumerator OnSceneEndLoad(); }
public interface IOnSceneBeginBootService { public IEnumerator OnSceneBeginBoot(); }
public interface IOnSceneEndBootService { public IEnumerator OnSceneEndBoot(); }
public interface IOnSceneBeginUnloadService { public IEnumerator OnSceneBeginUnload(); }
public interface IOnSceneEndUnloadService { public IEnumerator OnSceneEndUnload(); }

namespace Utils
{
    public class Services
    {
        private readonly HashSet<IOnProjectBeginLoadService> _onProjectBeginLoadServices = new();
        private readonly HashSet<IOnProjectEndLoadService> _onProjectEndLoadServices = new();
        private readonly HashSet<IOnSceneBeginLoadService> _onSceneBeginLoadServices = new();
        private readonly HashSet<IOnSceneEndLoadService> _onSceneEndLoadServices = new();
        private readonly HashSet<IOnSceneBeginBootService> _onSceneBeginBootServices = new();
        private readonly HashSet<IOnSceneEndBootService> _onSceneEndBootServices = new();
        private readonly HashSet<IOnSceneBeginUnloadService> _onSceneBeginUnloadServices = new();
        private readonly HashSet<IOnSceneEndUnloadService> _onSceneEndUnloadServices = new();
        
        public Services() => CacheServices();
        
        private void CacheServices()
        {
            var serviceTypes = Tools.ReflectionTool.GetSubclasses<ServiceBase>();
            foreach (var t in serviceTypes)
            {
                var instance = Activator.CreateInstance(t) as ServiceBase;
                var interfaces = t.GetInterfaces();
                foreach (var i in interfaces)
                {
                    if (i == typeof(IOnProjectBeginLoadService))
                    { _onProjectBeginLoadServices.Add(instance as IOnProjectBeginLoadService); continue; }

                    if (i == typeof(IOnProjectEndLoadService))
                    { _onProjectEndLoadServices.Add(instance as IOnProjectEndLoadService); continue; }
                    
                    if (i == typeof(IOnSceneBeginLoadService))
                    { _onSceneBeginLoadServices.Add(instance as IOnSceneBeginLoadService); continue;}

                    if (i == typeof(IOnSceneEndLoadService))
                    { _onSceneEndLoadServices.Add(instance as IOnSceneEndLoadService); continue; }

                    if (i == typeof(IOnSceneBeginBootService))
                    { _onSceneBeginBootServices.Add(instance as IOnSceneBeginBootService); continue; }

                    if (i == typeof(IOnSceneEndBootService))
                    { _onSceneEndBootServices.Add(instance as IOnSceneEndBootService); continue; }

                    if (i == typeof(IOnSceneBeginUnloadService))
                    { _onSceneBeginUnloadServices.Add(instance as IOnSceneBeginUnloadService); continue; }

                    if (i == typeof(IOnSceneEndUnloadService))
                    { _onSceneEndUnloadServices.Add(instance as IOnSceneEndUnloadService); continue; }
                }
            }
        }

        public IEnumerator OnProjectBeginLoad()
        {
            foreach (var service in _onProjectBeginLoadServices)
            { yield return service.OnProjectBeginLoad(); yield return null; }
        }

        public IEnumerator OnProjectEndLoad()
        {
            foreach (var service in _onProjectEndLoadServices)
            { yield return service.OnProjectEndLoad(); yield return null; }
        }
        
        public IEnumerator OnSceneBeginLoad()
        {
            foreach (var service in _onSceneBeginLoadServices)
            { yield return service.OnSceneBeginLoad(); yield return null; }
        }
        
        public IEnumerator OnSceneEndLoad()
        {
            foreach (var service in _onSceneEndLoadServices)
            { yield return service.OnSceneEndLoad(); yield return null; }
        }

        public IEnumerator OnSceneBeginBoot()
        {
            foreach (var service in _onSceneBeginBootServices)
            { yield return service.OnSceneBeginBoot(); yield return null; }
        }

        public IEnumerator OnSceneEndBoot()
        {
            foreach (var service in _onSceneEndBootServices)
            { yield return service.OnSceneEndBoot(); yield return null; }
        }

        public IEnumerator OnSceneBeginUnload()
        {
            foreach (var service in _onSceneBeginUnloadServices)
            { yield return service.OnSceneBeginUnload(); yield return null; }
        }

        public IEnumerator OnSceneEndUnload()
        {
            foreach (var service in _onSceneEndUnloadServices)
            { yield return service.OnSceneEndUnload(); yield return null; }
        }
    }
}