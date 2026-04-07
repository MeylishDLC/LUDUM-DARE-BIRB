using Cinemachine;
using Controller;
using InputSystem;
using Replay;
using UIScreens;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class MainSceneInstaller : MonoInstaller
    {
        [SerializeField] private InputListener inputListener;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private Counter counter;
        [SerializeField] private int defaultGameplaySeed = 42;

        public override void InstallBindings()
        {
            BindInputListener();
            BindCameras();
            BindPlayer();
            BindCounter();
            BindReplay();
        }
       
        private void BindInputListener()
        {
            Container.Bind<InputListener>().FromInstance(inputListener).AsSingle();
        }
        private void BindCameras()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
            Container.Bind<CinemachineVirtualCamera>().FromInstance(virtualCamera).AsSingle();
        }
        private void BindPlayer()
        {
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();
        }
        private void BindCounter()
        {
            Container.Bind<Counter>().FromInstance(counter).AsSingle();
        }

        private void BindReplay()
        {
            Container.BindInterfacesAndSelfTo<ReplayReadState>().AsSingle();
            Container.Bind<IRng>().FromMethod(CreateRng).AsSingle();
            Container.BindInterfacesAndSelfTo<ReplayCoordinator>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
        private IRng CreateRng()
        {
            var seed = ReplaySession.TryConsumePendingSeed(out var s) ? s : defaultGameplaySeed;
            return new SeededRng(seed);
        }
    }
}