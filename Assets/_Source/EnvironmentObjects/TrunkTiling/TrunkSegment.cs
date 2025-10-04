using UnityEngine;

namespace EnvironmentObjects.TrunkTiling
{
    public class TrunkSegment: MonoBehaviour
    {
        [field: SerializeField] public Transform TopPoint { get; private set; }
        [field: SerializeField] public Transform BottomPoint { get; private set; }
    }
}