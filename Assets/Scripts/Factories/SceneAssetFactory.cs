using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class SceneAssetFactory : BaseFactory, ISceneAssetFactory
    {
        public SceneAssetFactory(DiContainer container) : base(container)
        {
        }

        public T CreateAsset<T>() where T : MonoBehaviour
        {
            return Create<T>();
        }

        public T CreateAssetNotCached<T>() where T : MonoBehaviour
        {
            return CreateNotCached<T>();
        }
    }
}
