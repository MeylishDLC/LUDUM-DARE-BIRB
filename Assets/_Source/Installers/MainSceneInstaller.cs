using Cinemachine;
using Controller;
using Core;
using InputSystem;
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
        
        public override void InstallBindings()
        {
            BindSceneController();
            BindInputListener();
            BindCameras();
            BindPlayer();
            BindCounter();
        }
        private void BindSceneController()
        {
            Container.Bind<SceneController>().AsSingle();
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
    }
}