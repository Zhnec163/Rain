using System;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T Prefab;

    protected ObjectPool<T> Pool;
    protected int CountCreatedObjects;

    public virtual event Action<int, int> OnChangedCountObjects;

    protected void Init()
    {
        Pool = new ObjectPool<T>
        (
            createFunc: () => HandleActionOnCreate(),
            actionOnGet: obj => HandleActionOnGet(obj),
            actionOnRelease: obj => HandleActionOnRelease(obj),
            actionOnDestroy: obj => HandleActionOnDestroy(obj)
        );
    }

    protected abstract T HandleActionOnCreate();

    protected virtual void HandleActionOnGet(T spawnedObject)
    {
        if (spawnedObject.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        spawnedObject.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    protected virtual void HandleActionOnRelease(T spawnedObject)
    {
        spawnedObject.gameObject.SetActive(false);
    }

    protected virtual void HandleActionOnDestroy(T spawnedObject)
    {
        Destroy(spawnedObject.gameObject);
    }

    protected void Release(T spawnedObject)
    {
        Pool.Release(spawnedObject);
    }

    protected void CallOnChangedCountObjects()
    {
        OnChangedCountObjects?.Invoke(CountCreatedObjects, Pool.CountActive);
    }
}