using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Factories.Interfaces
{
    public interface ISceneAssetFactory
    {
        T CreateAsset<T>() where T : MonoBehaviour;
        T CreateAssetNotCached<T>() where T : MonoBehaviour;
    }
}
