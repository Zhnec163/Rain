using System;
using UnityEngine;

public class BombSpawner : Spawner<Bomb>
{
    [SerializeField] private BlobSpawner _blobSpawner;
    [SerializeField] private int _minLifetime = 2;
    [SerializeField] private int _maxLifetime = 5;
    
    private void Awake()
    {
        Init();
        _blobSpawner.OnBlobDisabled += HandleOnBlobDisabled;
    }

    private void OnDisable()
    {
        _blobSpawner.OnBlobDisabled -= HandleOnBlobDisabled;
    }

    protected override Bomb HandleActionOnCreate()
    {
        Bomb bomb = Instantiate(Prefab);
        bomb.Init(Release);
        IncrementCountCreatedObjects();
        return bomb;
    }
    
    protected override void HandleActionOnGet(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
        int maxValue = 100;
        bomb.Activate(maxValue / RandomHelper.GetRandomNumber(_minLifetime, _maxLifetime + 1) / maxValue);
        base.HandleActionOnGet(bomb);
    }

    private void HandleOnBlobDisabled(Blob blob)
    {
        Bomb bomb = Get();
        bomb.transform.position = blob.transform.position;
    }
}