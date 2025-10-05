namespace PoolSystem
{
    public interface IPool<T> where T : IPoolObject<T>
    {
        public void InitPool(T[] prefabs);
        public bool TryGetFromPool(out T instance);
        public void ReturnToPool(T instance);
    }
}