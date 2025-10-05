using System;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using SoundSystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UIScreens
{
    public class MainMenuScreen: MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button openSettingsButton;
        
        [Header("Settings")]
        [SerializeField] private RectTransform settingsScreen;
        [SerializeField] private UIPopup settingsPopup;
        [SerializeField] private Button closeSettingsButton;

        private SoundManager _soundManager;
        private SceneController _sceneController;
        private CancellationToken _ctOnDestroy;

        [Inject]
        public void Initialize(SoundManager soundManager, SceneController sceneController)
        {
            _soundManager = soundManager;
            _sceneController = sceneController;
        }
        private void Awake()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            settingsScreen.gameObject.SetActive(false);
            
            playButton.onClick.AddListener(StartGame);
            closeSettingsButton.onClick.AddListener(CloseSettings);
            openSettingsButton.onClick.AddListener(OpenSettings);
        }
        private void Start()
        {
            _soundManager.InitializeMusic(_soundManager.FmodEventsConfig.MenuMusic);
        }
        private void CloseSettings()
        {
            CloseSettingsAsync(_ctOnDestroy).Forget();
        }

        private void OpenSettings()
        {
            OpenSettingsAsync(_ctOnDestroy).Forget();
        }
        private async UniTask CloseSettingsAsync(CancellationToken cancellationToken)
        {
            try
            {
                await settingsPopup.PlayPopupAnimation(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                settingsPopup.transform.localScale = new Vector3(1, 1, 1);
            }
            finally
            {
                settingsScreen.gameObject.SetActive(false);
            }
        }

        private async UniTask OpenSettingsAsync(CancellationToken cancellationToken)
        {
            settingsScreen.gameObject.SetActive(true);
            try
            {
                await settingsPopup.PlayPopupAnimation(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                settingsPopup.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        private void StartGame()
        {
            playButton.interactable = false;
            _sceneController.LoadGameScene();
        }
        
    }
}