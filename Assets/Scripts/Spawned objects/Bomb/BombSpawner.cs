using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [field: SerializeField] private BlobSpawner _blobSpawner;
    [field: SerializeField] private int _minLifetime = 2;
    [field: SerializeField] private int _maxLifetime = 5;
    
    public Action<int, int> ChangedCountObjects;
    
    private void Awake()
    {
        Init();
        _blobSpawner.BlobDisabled += HandleBlobDisabled;
    }

    protected override Bomb CreateFunc()
    {
        Bomb bomb = Instantiate(SpawnedObject);
        bomb.Init(Release);
        CountCreatedObjects++;
        ChangedCountObjects.Invoke(CountCreatedObjects, Pool.CountActive);
        return bomb;
    }
    
    protected override void ActionOnGet(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
        bomb.Activate((100 / RandomHelper.GetRandomNumber(_minLifetime, _maxLifetime + 1)) / 100);
        base.ActionOnGet(bomb);
    }

    protected override void ActionOnDestroy(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }

    private void HandleBlobDisabled(Vector3 position)
    {
        Bomb bomb = Pool.Get();
        bomb.transform.position = position;
        ChangedCountObjects.Invoke(CountCreatedObjects, Pool.CountActive);
    }
}