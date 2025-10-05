using System;
using System.Threading;
using _Support.Demigiant.DOTween.Modules;
using Controller;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIScreens
{
    public class DeathScreen: MonoBehaviour
    {
        [SerializeField] private float fadeInDuration = 0.3f;
        [SerializeField] private float delay = 0.2f;
        [SerializeField] private Button restartButton;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private SceneController _sceneController;
        private PlayerController _player;
        private CancellationToken _ctOnDestroy;

        [Inject]
        public void Initialize(SceneController sceneController, PlayerController player)
        {
            _sceneController = sceneController;
            _player = player;
        }
        private void Awake()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            restartButton.onClick.AddListener(CloseDeathScreen);
            _player.OnPlayerDeath += ShowDeathScreen;
            gameObject.SetActive(false);
        }
        private void ShowDeathScreen()
        {
            _player.OnPlayerDeath -= ShowDeathScreen;
            ShowDeathScreenAsync(_ctOnDestroy).Forget();
        }
        private async UniTask ShowDeathScreenAsync(CancellationToken token)
        {
            gameObject.SetActive(true);
            canvasGroup.alpha = 0f;
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: token);
            await canvasGroup.DOFade(1f, fadeInDuration).ToUniTask(cancellationToken: token);
        }
        private void CloseDeathScreen()
        {
            restartButton.interactable = false;
            _sceneController.ReloadScene();
        }
    }
}