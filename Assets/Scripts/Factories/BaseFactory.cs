using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Factories
{
    public abstract class BaseFactory 
    {
        protected readonly DiContainer _container;

        private Dictionary<string, GameObject> _cache = new();                //Фабрика ассетов имеет кэш. Поскольку биндится она AsTransient, для каждой сцены будет свой экземпляр фабрики и свой кэш
                                                                            //Таким образом кэш сцены уничтожается вместе со сценой и не занимает память

        protected BaseFactory(DiContainer container)                        //Будем делать принудительный инжект создаваемых объектов, чтобы в них тоже работали Zenject зависимости
        {
            _container = container ?? throw new ArgumentNullException(nameof(container)); 
        }

        protected T Create<T>(bool cached = true)
        {
            var path = PrefabsPath.GetPathFor<T>();
            var instance = CreateFromPath(path, cached);
            return instance.GetComponent<T>();                              //Возвращается экземпляр монобеха
        }

        protected GameObject CreateFromPath(string prefabPath, bool cached = true)
        {
            GameObject gameObject;
            if (!_cache.TryGetValue(prefabPath, out gameObject))             //Если ассет уже загружался. он берется из кэша
            {               
                gameObject = Resources.Load<GameObject>(prefabPath);        //Иначе он загружается и кладется в кэш. Классика.
                if (cached)
                    _cache.Add(prefabPath, gameObject);                      //Ключом к кэшу является не сам gameobject, а путь к префабу
            }          

            var instance = GameObject.Instantiate(gameObject);
            _container.InjectGameObject(instance);

            return instance;
        }

        protected T CreateNotCached<T>()                                  //Однако есть возможность загрузить ассет не сохраняя его в кэше, например ассет - это модель левелбосса, которую нужно загрузать ровно один раз за сцену
        {
            return Create<T>(false);
        }
    }
}
