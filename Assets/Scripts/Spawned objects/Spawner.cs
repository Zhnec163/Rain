using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;
    
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _maxSize = 10;

    private int CountCreatedObjects;
    private ObjectPool<T> Pool;

    public event Action<int, int> OnChangedCountObjects;

    protected void Init()
    {
        Pool = new ObjectPool<T>
        (
            createFunc: () => HandleActionOnCreate(),
            actionOnGet: obj => HandleActionOnGet(obj),
            actionOnRelease: obj => HandleActionOnRelease(obj),
            actionOnDestroy: obj => HandleActionOnDestroy(obj),
            defaultCapacity: _poolCapacity,
            maxSize: _maxSize
        );
    }

    protected abstract T HandleActionOnCreate();

    protected virtual void HandleActionOnGet(T spawnedObject)
    {
        OnChangedCountObjects?.Invoke(CountCreatedObjects, Pool.CountActive);
        spawnedObject.transform.rotation = Quaternion.Euler(Vector3.zero);

        if (spawnedObject.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }

    protected virtual void HandleActionOnRelease(T spawnedObject)
    {
        spawnedObject.gameObject.SetActive(false);
    }

    protected virtual void HandleActionOnDestroy(T spawnedObject)
    {
        Destroy(spawnedObject.gameObject);
    }
    
    protected T Get()
    {
        return Pool.Get();
    }

    protected void Release(T spawnedObject)
    {
        Pool.Release(spawnedObject);
    }

    protected void IncrementCountCreatedObjects()
    {
        CountCreatedObjects++;
        OnChangedCountObjects?.Invoke(CountCreatedObjects, Pool.CountActive);
    }
}