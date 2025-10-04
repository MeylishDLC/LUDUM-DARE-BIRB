using UnityEngine;

namespace Controller
{
    public class PlayerModel
    {
        private float _lastJumpTime;
        private int _jumpPressCount;
        public float CalculateJumpForce(float baseJumpForce, float currentTime, float maxFrequencyBoost = 2f, float frequencyWindow = 0.3f)
        {
            if (currentTime - _lastJumpTime <= frequencyWindow)
            {
                _jumpPressCount++;
            }
            else
            {
                _jumpPressCount = 1;
            }

            _lastJumpTime = currentTime;

            var multiplier = Mathf.Clamp(1f + (_jumpPressCount - 1) * 0.25f, 1f, maxFrequencyBoost);
            return baseJumpForce * multiplier;
        }
    }
}