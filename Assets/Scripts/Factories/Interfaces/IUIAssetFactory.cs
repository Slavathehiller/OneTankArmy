using UnityEngine;

namespace Assets.Scripts.Factories.Interfaces
{
    public interface IUIAssetFactory
    {
        public T CreateAsset<T>(GameObject parent) where T : MonoBehaviour;
    }
}
