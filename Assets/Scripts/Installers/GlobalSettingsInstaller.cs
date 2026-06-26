using Assets.Scripts.Factories;
using Assets.Scripts.Planets;
using Assets.Scripts.Player;
using Assets.Scripts.SceneAssets;
using Assets.Scripts.SceneNavigation;
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
        Container.BindInterfacesTo<MissileFactory>().AsTransient();
        Container.BindInterfacesTo<MissilePool>().AsCached();
        Container.BindInterfacesTo<QuestItemsData>().AsSingle();
        Container.BindInterfacesTo<FloatTooltipManager>().AsSingle();
        Container.BindInterfacesTo<SceneNavigator>().AsSingle();
        Container.BindInterfacesTo<PlanetManager>().AsSingle();

        //Container.Bind<ILogger>().To<Logger>().AsCached();
        //Container.BindInterfacesTo<LocalizationManager>().AsSingle();
        //Container.BindInterfacesTo<TooltipManager>().AsSingle();        
    }
}

