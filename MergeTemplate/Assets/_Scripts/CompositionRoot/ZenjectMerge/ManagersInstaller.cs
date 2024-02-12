using Zenject;

public class ManagersInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SlotsManager>().AsSingle();
        Container.Bind<MergeItemsManager>().AsSingle();
    }
}
