using System;
using UnityEngine;

namespace EnvironmentObjects.Collectables
{
    public class CollectableItemTrigger: MonoBehaviour
    {
        public event Action OnTriggered;
        
        [SerializeField] private LayerMask playerLayer;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & playerLayer.value) != 0)
            {
                OnTriggered?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}