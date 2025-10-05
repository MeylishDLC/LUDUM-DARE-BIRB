using UnityEngine;

namespace SegmentsGeneration
{
    public class Segment: MonoBehaviour
    {
        [field: SerializeField] public Transform TopPoint { get; private set; }
        [field: SerializeField] public Transform BottomPoint { get; private set; }
    }
}