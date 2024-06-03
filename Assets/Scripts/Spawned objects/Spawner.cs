using UnityEngine;
using UnityEngine.Pool;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [field: SerializeField] protected T SpawnedObject;

    protected int CountCreatedObjects = 0;
    protected ObjectPool<T> Pool;
    
    protected void Init()
    {
        Pool = new ObjectPool<T>
        (
            createFunc: () => CreateFunc(),
            actionOnGet: obj => ActionOnGet(obj),
            actionOnRelease: obj => ActionOnRelease(obj),
            actionOnDestroy: obj => ActionOnDestroy(obj)
        );
    }

    protected abstract T CreateFunc();

    protected virtual void ActionOnGet(T spawnedObject)
    {
        if (spawnedObject.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
        
        spawnedObject.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    protected virtual void ActionOnRelease(T spawnedObject)
    {
        spawnedObject.gameObject.SetActive(false);
    }

    protected virtual void ActionOnDestroy(T spawnedObject)
    {
        Destroy(spawnedObject.gameObject);
    }

    protected void Release(T spawnedObject)
    {
        Pool.Release(spawnedObject);
    }
}