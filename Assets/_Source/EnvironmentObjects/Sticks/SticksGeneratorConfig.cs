using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    [CreateAssetMenu(fileName = "Sticks Generator Config", menuName = "Core/Sticks Generator Config")]
    public class SticksGeneratorConfig: ScriptableObject
    {
        [field: SerializeField] public float DistanceY { get; private set; } = 5f;
        [field: SerializeField] public int MaxPairs { get; private set; } = 7;
    }
}