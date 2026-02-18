using Assets.Scripts.Factories;
using Zenject;

namespace Assets.Scripts.Installers
{
    public class OutpostSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<VehicleFactory>().AsTransient();
        }
    }
}
