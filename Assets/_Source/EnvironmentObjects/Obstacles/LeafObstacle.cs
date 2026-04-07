using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Replay;
using UnityEngine;

namespace EnvironmentObjects.Obstacles
{
    public class LeafObstacle: BaseObstacle
    {
        [SerializeField] private GameObject visualsToMove;
        
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeedDuration = 2f;
        [SerializeField] private float moveDistance = 2f;

        [Header("Gizmos Settings")]
        [SerializeField] private Color gizmoColor = Color.green;
        
        private Vector2 _startPos;
        private Vector2 _rightPoint;
        private Vector2 _leftPoint;

        private CancellationToken _ctOnDestroy;
        private void Awake()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            
            _startPos = visualsToMove.transform.position;
            _leftPoint = _startPos + Vector2.left * moveDistance;
            _rightPoint = _startPos + Vector2.right * moveDistance;
            DoMovement(_ctOnDestroy).Forget();
        }
        private async UniTask DoMovement(CancellationToken token)
        {
            try
            {
                if (GetRandomDirection())
                {
                    while (true)
                    {
                        await MoveFromLeftToRight(token);
                    }  
                }
                while (true)
                {
                    await MoveFromRightToLeft(token);
                }

            }
            catch (OperationCanceledException)
            {
                //
            }
        }

        private bool GetRandomDirection()
        {
            var rng = GameplayRng.Instance;
            var rand = rng != null ? rng.RangeFloat(0f, 1f) : UnityEngine.Random.Range(0f, 1f);
            if (rand < 0.5f)
            {
                return true;
            }
            return false;
        }

        private async UniTask MoveFromLeftToRight(CancellationToken token)
        {
            await visualsToMove.transform.DOMoveX(_leftPoint.x, movementSpeedDuration).SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token);
            await visualsToMove.transform.DOMoveX(_rightPoint.x, movementSpeedDuration).SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token);
        }

        private async UniTask MoveFromRightToLeft(CancellationToken token)
        {
            await visualsToMove.transform.DOMoveX(_rightPoint.x, movementSpeedDuration).SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token);
            await visualsToMove.transform.DOMoveX(_leftPoint.x, movementSpeedDuration).SetEase(Ease.Linear)
                .ToUniTask(cancellationToken: token);
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = gizmoColor;
            Vector3 pos;
            if (Application.isPlaying)
            {
                pos = _startPos;
            }
            else
            {
                pos = visualsToMove.transform.position;
            }

            var left = pos + Vector3.left * moveDistance;
            var right = pos + Vector3.right * moveDistance;

            Gizmos.DrawLine(left, right);
            Gizmos.DrawSphere(left, 0.1f);
            Gizmos.DrawSphere(right, 0.1f);
        }
    }
}