using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Factories
{
    public static class PrefabsPath
    {

        private static Dictionary<Type, string> _pathes = new();

        public static void Register(Type type, string path)
        {
            if (_pathes.ContainsKey(type))
            {
                Debug.LogError($"Type {type.FullName} already registered in PrefabPath.");
                return;
            }

            _pathes.Add(type, path);
        }

        public static string GetPathFor<T>()
        {
            string result;
            if (!_pathes.TryGetValue(typeof(T), out result))
            {
                Debug.LogError($"Type {typeof(T).FullName} not registered in PrefabPath.");
            }
            return result;
        }


        public static void InitPathes()
        {
            Register(typeof(Explosion), "Prefabs/Explosion");
            Register(typeof(BoomFlea), "Prefabs/BoomFlea");
            Register(typeof(AcidGoo), "Prefabs/AcidGoo");
            Register(typeof(ToxicGoo), "Prefabs/ToxicGoo");
            Register(typeof(BigGreenGoo), "Prefabs/BigGreenGoo");            
            Register(typeof(Flame), "Prefabs/Flame");
            Register(typeof(Beetle), "Prefabs/Vehicle/Beetle");
            Register(typeof(DianBao), "Prefabs/Vehicle/DianBao");
            Register(typeof(Fury), "Prefabs/Vehicle/Fury");
        }
    }
}
