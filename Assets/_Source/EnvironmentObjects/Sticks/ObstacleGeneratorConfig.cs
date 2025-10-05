using UnityEngine;

namespace EnvironmentObjects.Sticks
{
    [CreateAssetMenu(fileName = "Obstacle Generator Config", menuName = "Core/Obstacle Generator Config")]
    public class ObstacleGeneratorConfig: ScriptableObject
    {
        [field: SerializeField] public float DistanceY { get; private set; } = 5f;
        [field: SerializeField] public int MaxPairs { get; private set; } = 7;
    }
}