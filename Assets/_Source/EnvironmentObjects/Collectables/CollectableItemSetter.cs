using System;
using EnvironmentObjects.Sticks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EnvironmentObjects.Collectables
{
    public class CollectableItemSetter : MonoBehaviour
    {
        [Range(0, 100)] [SerializeField] private int itemPlaceChance = 50;
        [SerializeField] private StickPair stickPair;

        private CollectableItemPool _itemPool;
        private CollectableItem _currentItem;
        private Transform _origParent;
        private bool _isInitialized;

        public void InitializePool(CollectableItemPool pool)
        {
            if (_isInitialized)
            {
                return;
            }

            _itemPool = pool;
            stickPair.OnObjectEnabled += PlaceRandomItem;
            stickPair.OnObjectDisabled += RemoveRandomItem;
            _isInitialized = true;
        }
        private void PlaceRandomItem(StickPair _)
        {
            if (_currentItem != null)
            {
                RemoveRandomItem(stickPair);
            }

            if (!CheckPlaceChance())
            {
                return;
            }

            if (_itemPool.TryGetFromPool(out var item))
            {
                _currentItem = item;
                _origParent = _currentItem.transform.parent;

                var randomPoint = GetRandomPoint();
                _currentItem.transform.SetParent(randomPoint);
                _currentItem.transform.position = randomPoint.position;
                _currentItem.gameObject.SetActive(true);

                _currentItem.OnCollected += StopPlacingItems;
            }
        }
        private void RemoveRandomItem(StickPair _)
        {
            if (_currentItem == null)
            {
                return;
            }

            _currentItem.OnCollected -= StopPlacingItems;
            _currentItem.gameObject.SetActive(false);
            _currentItem.transform.SetParent(_origParent);
            _currentItem = null;
        }
        private bool CheckPlaceChance()
        {
            return Random.Range(0, 100) < itemPlaceChance;
        }
        private Transform GetRandomPoint()
        {
            return stickPair.ItemSpawnPoints[
                Random.Range(0, stickPair.ItemSpawnPoints.Length)
            ];
        }
        private void StopPlacingItems()
        {
            if (_currentItem)
            {
                _currentItem.OnCollected -= StopPlacingItems;
            }

            _currentItem = null;
        }
    }
}