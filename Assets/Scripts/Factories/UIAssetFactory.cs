using Assets.Scripts.Factories.Interfaces;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Factories
{
    public class UIAssetFactory : BaseFactory, IUIAssetFactory
    {
        public UIAssetFactory(DiContainer container) : base(container) {}
        public T CreateAsset<T>(GameObject parent) where T : MonoBehaviour
        {
            var result = Create<T>();

            if (parent != null)
                result.transform.SetParent(parent.transform);
            result.transform.localPosition = Vector3.zero;

            return result;
        }

    }
}
