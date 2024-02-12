using Zenject;

namespace CodeBase.CompositionRoot
{
    public class SceneContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<SceneObjectsProvider>().AsSingle();
        }
    }
}