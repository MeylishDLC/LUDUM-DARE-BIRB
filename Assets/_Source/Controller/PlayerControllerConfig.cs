using UnityEngine;

namespace Controller
{
    [CreateAssetMenu(fileName = "Player Controller Config", menuName = "Core/Player Controller Config")]
    public class PlayerControllerConfig: ScriptableObject
    {
        [field: Header("Physics Settings")]
        [field: SerializeField] public float Gravity { get; private set; } = -30f;
        [field: SerializeField] public float MaxFallSpeed { get; private set; } = -25f;
        
        [field: Header("Movement Settings")]
        [field: SerializeField] public float HorizontalMoveSpeed { get; private set; } = 8f;
        [field: SerializeField] public float Acceleration { get; private set; } = 20f;
        [field: SerializeField] public float Deceleration { get; private set; } = 25f;

        [field: Header("Jump Settings")]
        [field: SerializeField] public float MinJumpForce { get; private set; } = 10f;
        [field: SerializeField] public float JumpForce { get; private set; } = 18f;
        [field: SerializeField] public float MaxJumpHoldTime { get; private set; } = 0.25f;
    }
}