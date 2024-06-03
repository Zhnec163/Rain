using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [field: SerializeField] private BlobSpawner _blobSpawner;
    [field: SerializeField] private int _minLifetime = 2;
    [field: SerializeField] private int _maxLifetime = 5;
    
    private void Awake()
    {
        Init();
        _blobSpawner.BlobDisabled += HandleBlobDisabled;
    }

    protected override Bomb HandleActionOnCreate()
    {
        Bomb bomb = Instantiate(Prefab);
        bomb.Init(Release);
        CountCreatedObjects++;
        CallOnChangedCountObjects();
        return bomb;
    }
    
    protected override void HandleActionOnGet(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
        bomb.Activate((100 / RandomHelper.GetRandomNumber(_minLifetime, _maxLifetime + 1)) / 100);//
        base.HandleActionOnGet(bomb);
    }

    protected override void HandleActionOnDestroy(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }

    private void HandleBlobDisabled(Vector3 position)
    {
        Bomb bomb = Pool.Get();
        bomb.transform.position = position;
        CallOnChangedCountObjects();
    }
}