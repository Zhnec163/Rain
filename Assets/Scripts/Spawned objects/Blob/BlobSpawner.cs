using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BlobSpawner : Spawner<Blob>
{
    [SerializeField] private float _spawnFrequency = 1F;
    [SerializeField] private Ground _ground;

    public event Action<Blob> OnBlobDisabled;

    private WaitForSeconds _delay;
    private bool _isRainRun = true;

    private void Awake()
    {
        Init();
        _delay = new WaitForSeconds(_spawnFrequency);
        StartCoroutine(StartRain());
    }

    private void OnDestroy()
    {
        _isRainRun = false;
    }

    protected override Blob HandleActionOnCreate()
    {
        Blob blob = Instantiate(Prefab, RandomHelper.GetRandomPositionOver(_ground.transform, transform), Quaternion.identity);
        blob.Init(ReturnDroppedBlob);
        IncrementCountCreatedObjects();
        return blob;
    }

    protected override void HandleActionOnGet(Blob blob)
    {
        blob.gameObject.SetActive(true);
        blob.transform.position = RandomHelper.GetRandomPositionOver(_ground.transform, transform);
        blob.ResetState();
        base.HandleActionOnGet(blob);
    }

    private void ReturnDroppedBlob(Blob blob)
    {
        float minLifeTime = 2F;
        float maxLifeTime = 6F;
        float lifeTime = RandomHelper.GetRandomNumber(minLifeTime, maxLifeTime);
        StartCoroutine(ReturnDropThrough(blob, lifeTime));
    }

    private IEnumerator StartRain()
    {
        while (_isRainRun)
        {
            Get();
            yield return _delay;
        }
    }

    private IEnumerator ReturnDropThrough(Blob blob, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        OnBlobDisabled.Invoke(blob);
        Release(blob);
    }
}