using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using InputSystem;
using R3;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialWindow: MonoBehaviour
    {
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float scaleOnClose = 1.15f;
        [SerializeField] Ease ease = Ease.Linear;
        
        private CancellationToken _ctOnDestroy;
        private InputListener _listener;
        private SceneController _sceneController;
        private IDisposable _jumpStartedSubscription;

        [Inject]
        public void Initialize(InputListener listener, SceneController sceneController)
        {
            _listener = listener;
            _sceneController = sceneController;
        }
        private void Awake()
        {
            if (_sceneController.CurrentSceneIndex == _sceneController.LastSceneIndex)
            {
                gameObject.SetActive(false);
                return;
            }
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            _jumpStartedSubscription = _listener.JumpStartedStream.Subscribe(_ => CloseWindow());
        }
        
        private void CloseWindow()
        {
            _jumpStartedSubscription?.Dispose();
            _jumpStartedSubscription = null;
            gameObject.transform.DOScale(new Vector3(scaleOnClose, scaleOnClose, scaleOnClose), animationDuration)
                .SetEase(ease).ToUniTask(cancellationToken: _ctOnDestroy)
                .ContinueWith(() => Destroy(gameObject)).Forget();
        }
        private void OnDestroy()
        {
            _jumpStartedSubscription?.Dispose();
        }
    }
}