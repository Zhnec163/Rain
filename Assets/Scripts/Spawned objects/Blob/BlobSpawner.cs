using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BlobSpawner : Spawner<Blob>
{
    [field: SerializeField] private float _spawnFrequency = 1F;
    [field: SerializeField] private Ground _ground;
    [field: SerializeField] private int _poolCapacity = 5;
    [field: SerializeField] private int _maxSize = 10;
    
    public Action<Vector3> BlobDisabled;
    public Action<int, int> ChangedCountObjects;
    
    private void Awake()
    {
        Pool = new ObjectPool<Blob>
        (
            createFunc: () => CreateFunc(),
            actionOnGet: obj => ActionOnGet(obj),
            actionOnRelease: obj => ActionOnRelease(obj),
            actionOnDestroy: obj => ActionOnDestroy(obj),
            defaultCapacity: _poolCapacity,
            maxSize: _maxSize
        );
        
        StartCoroutine(StartRain());
    }

    protected override Blob CreateFunc()
    {
        Blob blob = Instantiate(SpawnedObject, RandomHelper.GetRandomPositionOver(_ground.transform, transform), Quaternion.identity);
        blob.Init(ReturnDroppedBlob);
        CountCreatedObjects++;
        ChangedCountObjects.Invoke(CountCreatedObjects, Pool.CountActive);
        return blob;
    }
        
    protected override void ActionOnGet(Blob blob)
    {
        blob.gameObject.SetActive(true);
        base.ActionOnGet(blob);
        blob.transform.position = RandomHelper.GetRandomPositionOver(_ground.transform, transform);
        blob.ResetState();
        ChangedCountObjects.Invoke(CountCreatedObjects, Pool.CountActive);
    }
    
    protected override void ActionOnRelease(Blob blob)
    {
        blob.transform.rotation = Quaternion.Euler(Vector3.zero);
        base.ActionOnRelease(blob);
    }
    
    public void ReturnDroppedBlob(Blob blob)
    {
        float minLifeTime = 2F;
        float maxLifeTime = 6F;
        float lifeTime = RandomHelper.GetRandomNumber(minLifeTime, maxLifeTime);
        StartCoroutine(ReturnDropThrough(blob, lifeTime));
    }

    private IEnumerator StartRain()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnFrequency);
        bool isRun = true;
    
        while (isRun)
        {
            Pool.Get();
            yield return waitForSeconds;
        }
    }
    
    private IEnumerator ReturnDropThrough(Blob blob, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        Vector3 position = blob.transform.position;
        BlobDisabled.Invoke(position);
        Release(blob);
    }
}
