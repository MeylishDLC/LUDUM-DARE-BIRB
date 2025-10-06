using System;
using Controller;
using Core;
using EnvironmentObjects.Collectables;
using SoundSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace UIScreens
{
    public class Counter: MonoBehaviour
    {
        [SerializeField] private TMP_Text counterText;

        private int _count;
        private PlayerController _player;
        private ScoreSaver _scoreSaver;
        private SoundManager _soundManager;

        [Inject]
        public void Initialize(PlayerController player, ScoreSaver scoreSaver, SoundManager soundManager)
        {
            _player = player;
            _scoreSaver = scoreSaver;
            _soundManager = soundManager;
        }
        private void Awake()
        {
            counterText.text = _count.ToString();
            _player.OnPlayerDeath += SaveScore; 
            CollectableItem.OnItemCollectedGeneral += UpdateText;
            CollectableItem.OnItemCollectedGeneral += PlayItemCollectedSound;
        }
        private void OnDestroy()
        {
            CollectableItem.OnItemCollectedGeneral -= UpdateText;
            CollectableItem.OnItemCollectedGeneral -= PlayItemCollectedSound;
        }
        public int GetCurrentScore() => _count;
        private void UpdateText()
        {
            _count++;
            counterText.text = _count.ToString();
        }
        private void SaveScore()
        {
            var maxScore = _scoreSaver.GetMaxScore();
            if (_count > maxScore)
            {
                _scoreSaver.SaveMaxScore(_count);
            }
        }
        private void PlayItemCollectedSound()
        {
            _soundManager.PlayOneShot(_soundManager.FmodEventsConfig.ItemCollectedSound);
        }
    }
}