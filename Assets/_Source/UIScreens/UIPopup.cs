using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace UIScreens
{
    public class UIPopup: MonoBehaviour
    {
        [SerializeField] private float animationDuration = 0.2f;
        [SerializeField] private float scaleOnAppear = 1.2f;
        [SerializeField] private Ease ease = Ease.InOutQuad;
        
        private RectTransform _rectTransform;
        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        public UniTask PlayPopupAnimation(CancellationToken cancellationToken)
        {
            return _rectTransform.DOScale(scaleOnAppear, animationDuration)
                .SetEase(ease).SetLoops(2, LoopType.Yoyo)
                .ToUniTask(cancellationToken: cancellationToken);
        }
    }
}