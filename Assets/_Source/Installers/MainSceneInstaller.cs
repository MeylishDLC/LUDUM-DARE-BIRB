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
        [SerializeField] private PlayerController playerController;
        public override void InstallBindings()
        {
            BindInputListener();
            BindMainCamera();
            BindPlayer();
        }
        private void BindInputListener()
        {
            Container.Bind<InputListener>().FromInstance(inputListener).AsSingle();
        }
        private void BindMainCamera()
        {
            Container.Bind<Camera>().FromInstance(mainCamera).AsSingle();
        }
        private void BindPlayer()
        {
            Container.Bind<PlayerController>().FromInstance(playerController).AsSingle();
        }
    }
}