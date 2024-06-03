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
    
    public event Action<Vector3> BlobDisabled;
    
    private void Awake()
    {
        Pool = new ObjectPool<Blob>
        (
            createFunc: () => HandleActionOnCreate(),
            actionOnGet: obj => HandleActionOnGet(obj),
            actionOnRelease: obj => HandleActionOnRelease(obj),
            actionOnDestroy: obj => HandleActionOnDestroy(obj),
            defaultCapacity: _poolCapacity,
            maxSize: _maxSize
        );
        
        StartCoroutine(StartRain());
    }

    protected override Blob HandleActionOnCreate()
    {
        Blob blob = Instantiate(Prefab, RandomHelper.GetRandomPositionOver(_ground.transform, transform), Quaternion.identity);
        blob.Init(ReturnDroppedBlob);
        CountCreatedObjects++;
        CallOnChangedCountObjects();
        return blob;
    }
        
    protected override void HandleActionOnGet(Blob blob)
    {
        blob.gameObject.SetActive(true);
        base.HandleActionOnGet(blob);
        blob.transform.position = RandomHelper.GetRandomPositionOver(_ground.transform, transform);
        blob.ResetState();
        CallOnChangedCountObjects();
    }
    
    protected override void HandleActionOnRelease(Blob blob)
    {
        blob.transform.rotation = Quaternion.Euler(Vector3.zero);
        base.HandleActionOnRelease(blob);
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
