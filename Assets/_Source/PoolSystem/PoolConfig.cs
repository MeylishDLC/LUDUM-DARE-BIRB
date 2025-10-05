using UnityEngine;

namespace PoolSystem
{
    [CreateAssetMenu(fileName = "Pool Config", menuName = "Core/Pool Config")]
    public class PoolConfig: ScriptableObject
    {
        [field: SerializeField] public int InitialPoolSize { get; private set; }
        [field: SerializeField] public int MaxPoolSize { get; private set; }
        [field: SerializeField] public GameObject[] Prefabs { get; private set; }
    }
}