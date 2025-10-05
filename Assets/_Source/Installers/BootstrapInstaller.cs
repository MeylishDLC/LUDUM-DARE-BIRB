using Core;
using Zenject;

namespace Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScoreSaver();
        }
        private void BindScoreSaver()
        {
            Container.Bind<ScoreSaver>().AsSingle();
        }
    }
}