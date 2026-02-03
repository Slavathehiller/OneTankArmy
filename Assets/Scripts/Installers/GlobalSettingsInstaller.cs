using Assets.Scripts;
using Assets.Scripts.Factories;
using Assets.Scripts.Interfaces;
using Assets.Scripts.VFX.Interfaces;
using Zenject;

public class GlobalSettingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<SceneAssetFactory>().AsTransient();
        Container.BindInterfacesTo<UIAssetFactory>().AsTransient();
        Container.BindInterfacesTo<VFXManager>().AsSingle();
        //Container.Bind<ILogger>().To<Logger>().AsCached();
        //Container.BindInterfacesTo<LocalizationManager>().AsSingle();
        //Container.BindInterfacesTo<TooltipManager>().AsSingle();        
    }
}

