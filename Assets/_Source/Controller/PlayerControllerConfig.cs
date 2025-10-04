using UnityEngine;

namespace Controller
{
    [CreateAssetMenu(fileName = "Player Controller Config", menuName = "Core/Player Controller Config")]
    public class PlayerControllerConfig: ScriptableObject
    {
        [field: SerializeField] public float Gravity { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField] public float HorizontalMoveSpeed { get; private set; }
        [field: SerializeField] public float Acceleration { get; private set; } = 15f;
        [field: SerializeField] public float Deceleration { get; private set; } = 20f;
        
        [field: Header("Jump Settings")]
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float MaxJumpHoldTime { get; private set; } = 0.2f;
        [field: SerializeField] public float MinJumpForce { get; private set; } = 5f;
        [field: SerializeField] public float MaxFallSpeed { get; private set; } 
    }
}