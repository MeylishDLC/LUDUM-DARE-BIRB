using System;
using Controller;
using Core;
using EnvironmentObjects.Collectables;
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

        [Inject]
        public void Initialize(PlayerController player, ScoreSaver scoreSaver)
        {
            _player = player;
            _scoreSaver = scoreSaver;
        }
        private void Awake()
        {
            counterText.text = _count.ToString();
            _player.OnPlayerDeath += SaveScore; 
            CollectableItem.OnItemCollectedGeneral += UpdateText;
        }
        private void OnDestroy()
        {
            CollectableItem.OnItemCollectedGeneral -= UpdateText;
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
    }
}