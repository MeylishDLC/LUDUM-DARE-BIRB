using System;

namespace PoolSystem
{
    public interface IPoolObject<out T>
    {
        public event Action<T> OnObjectDisabled;
    }
}