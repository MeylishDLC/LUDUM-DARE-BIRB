using System;
using System.Threading;
using _Support.Demigiant.DOTween.Modules;
using Controller;
using Core;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SoundSystem;
using TMPro;
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
        
        [Header("Score texts")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text maxScoreText;
        
        private SceneController _sceneController;
        private PlayerController _player;
        private SoundManager _soundManager;
        private CancellationToken _ctOnDestroy;
        private Counter _counter;
        private ScoreSaver _scoreSaver;

        [Inject]
        public void Initialize(SceneController sceneController, PlayerController player, Counter counter, 
            ScoreSaver scoreSaver, SoundManager soundManager)
        {
            _sceneController = sceneController;
            _soundManager = soundManager;
            _player = player;
            _counter = counter;
            _scoreSaver = scoreSaver;
        }
        private void Awake()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            restartButton.onClick.AddListener(CloseDeathScreen);
            _player.OnPlayerDeath += ShowDeathScreen;
            gameObject.SetActive(false);
            
            scoreText.text = "0";
            maxScoreText.text = "0";
        }
        private void ShowDeathScreen()
        {
            _player.OnPlayerDeath -= ShowDeathScreen;
            ShowDeathScreenAsync(_ctOnDestroy).Forget();
        }
        private async UniTask ShowDeathScreenAsync(CancellationToken token)
        {
            UpdateScoreTexts();
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
        private void UpdateScoreTexts()
        {
            scoreText.text = _counter.GetCurrentScore().ToString();
            maxScoreText.text = _scoreSaver.GetMaxScore().ToString();
        }
    }
}