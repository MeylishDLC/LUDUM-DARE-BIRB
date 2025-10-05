using System;
using EnvironmentObjects.Sticks;
using PoolSystem;
using UnityEngine;

namespace EnvironmentObjects.Obstacles
{
    public abstract class BaseObstacle: MonoBehaviour, IPoolObject<BaseObstacle>
    {
        public event Action<BaseObstacle> OnObjectDisabled;
        public event Action<BaseObstacle> OnObjectEnabled;
        
        protected virtual void OnEnable()
        {
            OnObjectEnabled?.Invoke(this);
        }
        protected virtual void OnDisable()
        {
            OnObjectDisabled?.Invoke(this);
        }
    }
}