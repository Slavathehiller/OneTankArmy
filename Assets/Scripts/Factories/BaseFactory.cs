using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Factories
{
    public abstract class BaseFactory 
    {
        protected readonly DiContainer _container;

        private Dictionary<Type, GameObject> _cache = new();                //Фабрика ассетов имеет кэш. Поскольку биндится она AsTransient, для каждой сцены будет свой экземпляр фабрики и свой кэш
                                                                            //Таким образом кэш сцены уничтожается вместе со сценой и не занимает память

        protected BaseFactory(DiContainer container)                        //Будем делать принудительный инжект создаваемых объектов, чтобы в них тоже работали Zenject зависимости
        {
            _container = container ?? throw new ArgumentNullException(nameof(container)); 
        }

        protected T Create<T>(bool cached = true)
        {
            GameObject gameObject;
            if (!_cache.TryGetValue(typeof(T), out gameObject))             //Если ассет уже загружался. он берется из кэша
            {   
                var path = PrefabsPath.GetPathFor<T>();                     //Иначе он загружается и кладется в кэш. Классика.
                gameObject = Resources.Load<GameObject>(path);
                if (cached)
                    _cache.Add(typeof(T), gameObject);                      //Ключом к кэшу является не сам gameobject, а класс монобеха
            }

            var instance = GameObject.Instantiate(gameObject);
            _container.InjectGameObject(instance);

            return instance.GetComponent<T>();                              //Возвращается экземпляр монобеха
        }

        protected T CreateNotCached<T>()                                  //Однако есть возможность загрузить ассет не сохраняя его в кэше, например ассет - это модель левелбосса, которую нужно загрузать ровно один раз за сцену
        {
            return Create<T>(false);
        }
    }
}
