using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Collectables
{
    public class CollectableItem: MonoBehaviour, IPoolObject<CollectableItem>
    {
        public event Action<CollectableItem> OnObjectDisabled;
        public event Action OnCollected;
        public static event Action OnItemCollectedGeneral;
        
        [SerializeField] private LayerMask playerMask;
        [SerializeField] private float scaleOnDisappear;
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] Ease ease = Ease.InOutSine;

        private bool _isCollected;
        private CancellationToken _ctOnDestroy;
        private void Awake()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
        }
        private void OnEnable()
        {
            _isCollected = false;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isCollected)
            {
                return;
            }
            if (((1 << other.gameObject.layer) & playerMask.value) != 0)
            {
                OnCollected?.Invoke();
                OnItemCollectedGeneral?.Invoke();

                _isCollected = true;
                AnimateOnItemCollectedAsync(_ctOnDestroy).Forget();
            }
        }
        private async UniTask AnimateOnItemCollectedAsync(CancellationToken token)
        {
            await transform.DOScale(scaleOnDisappear, animationDuration)
                .SetEase(ease).SetLoops(2, LoopType.Yoyo).ToUniTask(cancellationToken: token);
            gameObject.SetActive(false);
        }
        private void OnDisable()
        {
            OnObjectDisabled?.Invoke(this);
        }
    }
} 