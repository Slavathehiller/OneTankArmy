using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface ISceneAssetFactory
    {
        public T CreateAsset<T>() where T : MonoBehaviour;
        public T CreateAssetNotCached<T>() where T : MonoBehaviour;
    }
}
