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
            BindSceneController();
            BindScoreSaver();
        }
        private void BindSceneController()
        {
            var controller = new SceneController(1);
            Container.Bind<SceneController>().FromInstance(controller).AsSingle();
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