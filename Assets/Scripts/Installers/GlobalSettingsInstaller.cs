using Assets.Scripts.Factories;
using Assets.Scripts.Player;
using Zenject;

public class GlobalSettingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<SceneAssetFactory>().AsTransient();
        Container.BindInterfacesTo<UIAssetFactory>().AsTransient();
        Container.BindInterfacesTo<VFXManager>().AsSingle();
        Container.BindInterfacesTo<PlayerSettings>().AsSingle();
        Container.BindInterfacesTo<ContractsManager>().AsSingle();

        //Container.Bind<ILogger>().To<Logger>().AsCached();
        //Container.BindInterfacesTo<LocalizationManager>().AsSingle();
        //Container.BindInterfacesTo<TooltipManager>().AsSingle();        
    }
}

