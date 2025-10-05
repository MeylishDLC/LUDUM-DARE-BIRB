using Cinemachine;
using UnityEngine;

namespace CameraLogic
{
    [ExecuteAlways]
    [SaveDuringPlay]
    [AddComponentMenu("Cinemachine/Extensions/Confine Down Only")]
    public class ConfineDownOnly : CinemachineExtension
    {
        [SerializeField] private float minY = -10f;

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Body)
            {
                var pos = state.RawPosition;
                if (pos.y < minY)
                {
                    pos.y = minY;
                }
                state.RawPosition = pos;
            }
        }
    }
}