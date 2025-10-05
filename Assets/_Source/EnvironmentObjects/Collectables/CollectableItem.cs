using System;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Collectables
{
    public class CollectableItem: MonoBehaviour, IPoolObject<CollectableItem>
    {
        public event Action<CollectableItem> OnObjectDisabled;
        public event Action OnCollected;
        
        [SerializeField] private int points;
        [SerializeField] private LayerMask playerMask;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & playerMask.value) != 0)
            {
                OnCollected?.Invoke();
                Debug.Log("Collected");
                gameObject.SetActive(false);
            }
        }
        private void OnDisable()
        {
            OnObjectDisabled?.Invoke(this);
        }
    }
} 