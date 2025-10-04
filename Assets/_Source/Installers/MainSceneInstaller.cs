using Cinemachine;
using Controller;
using InputSystem;
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
        public override void InstallBindings()
        {
            BindInputListener();
            BindCameras();
            BindPlayer();
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
    }
}