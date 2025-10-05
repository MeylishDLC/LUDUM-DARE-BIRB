using Core;
using SoundSystem;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField] private SoundManager soundManagerPrefab;

        private SoundManager _soundManager;
        public override void InstallBindings()
        {
            BindSoundManager();
            BindScoreSaver();
        }
        private void BindScoreSaver()
        {
            Container.Bind<ScoreSaver>().AsSingle();
        }
        private void BindSoundManager()
        {
            _soundManager = Container.InstantiatePrefabForComponent<SoundManager>(soundManagerPrefab);
            Container.Bind<SoundManager>().FromInstance(_soundManager).AsSingle();
        }
    }
}