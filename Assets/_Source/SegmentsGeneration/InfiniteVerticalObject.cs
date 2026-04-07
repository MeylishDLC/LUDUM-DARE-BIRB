using Controller;
using Replay;
using UnityEngine;
using Zenject;

namespace SegmentsGeneration
{
    public class InfiniteVerticalObject: MonoBehaviour
    {
        [SerializeField] private int segmentCount = 6;
        [SerializeField] private float cameraOffset = 50f;
        [SerializeField] private Segment[] segmentPrefabs;

        private Segment[] _segments;
        private Transform _player;
        private IRng _rng;

        [Inject]
        public void Initialize(PlayerController playerController, IRng rng)
        {
            _player = playerController.transform;
            _rng = rng;
        }
        private void Start()
        {
            _segments = new Segment[segmentCount];
            SpawnStartSegments();
        }
        private void Update()
        {
            var cameraBottom = _player.position.y - cameraOffset;
            var cameraTop = _player.position.y + cameraOffset;

            foreach (var seg in _segments)
            {
                if (seg.BottomPoint.position.y < cameraBottom)
                {
                    var topSegment = GetTopSegment();
                    ReplaceSegmentAbove(seg, topSegment);
                }
                else if (seg.TopPoint.position.y > cameraTop)
                {
                    var bottomSegment = GetBottomSegment();
                    ReplaceSegmentBelow(seg, bottomSegment);
                }
            }
        }
        private void ReplaceSegmentAbove(Segment oldSeg, Segment previousSeg)
        {
            Destroy(oldSeg.gameObject);

            var newSeg = Instantiate(GetRandomPrefab(), transform);

            AttachSegment(newSeg, previousSeg);

            ReplaceSegmentReference(oldSeg, newSeg);
        }
        private void ReplaceSegmentBelow(Segment oldSeg, Segment previousSeg)
        {
            Destroy(oldSeg.gameObject);

            var newSeg = Instantiate(GetRandomPrefab(), transform);
            AttachSegmentBelow(newSeg, previousSeg);
            ReplaceSegmentReference(oldSeg, newSeg);
        }
        private void ReplaceSegmentReference(Segment oldSeg, Segment newSeg)
        {
            for (int i = 0; i < _segments.Length; i++)
            {
                if (_segments[i] == oldSeg)
                {
                    _segments[i] = newSeg;
                    break;
                }
            }
        }
        private Segment GetRandomPrefab()
        {
            return segmentPrefabs[_rng.RangeInt(0, segmentPrefabs.Length)];
        }
        private void AttachSegment(Segment newSeg, Segment previousSeg)
        {
            var offset = previousSeg.TopPoint.position - newSeg.BottomPoint.position;
            newSeg.transform.position += offset;
        }
        private void AttachSegmentBelow(Segment newSeg, Segment previousSeg)
        {
            var offset = previousSeg.BottomPoint.position - newSeg.TopPoint.position;
            newSeg.transform.position += offset;
        }
        private Segment GetTopSegment()
        {
            var top = _segments[0];
            foreach (var seg in _segments)
            {
                if (seg.transform.position.y > top.transform.position.y)
                {
                    top = seg;
                }
            }

            return top;
        }
        private Segment GetBottomSegment()
        {
            var bottom = _segments[0];
            var bottomY = _segments[0].BottomPoint.position.y;

            for (int i = 1; i < _segments.Length; i++)
            {
                var y = _segments[i].BottomPoint.position.y;
                if (y < bottomY)
                {
                    bottomY = y;
                    bottom = _segments[i];
                }
            }

            return bottom;
        }
        private void SpawnStartSegments()
        {
            for (var i = 0; i < segmentCount; i++)
            {
                var seg = Instantiate(GetRandomPrefab(), transform);
                if (i == 0)
                {
                    seg.transform.localPosition = Vector3.zero;
                }
                else
                {
                    AttachSegment(seg, _segments[i - 1]);
                }

                _segments[i] = seg;
            }
        }
    }
}