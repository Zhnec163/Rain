using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BlobSpawner : Spawner<Blob>
{
    [SerializeField] private float _spawnFrequency = 1F;
    [SerializeField] private Ground _ground;

    public event Action<Blob> BlobDisabled;

    private void Awake()
    {
        Init();
        StartCoroutine(StartRain());
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
        // TODO
        WaitForSeconds waitForSeconds = new WaitForSeconds(_spawnFrequency);
        bool isRun = true;

        while (isRun)
        {
            Get();
            yield return waitForSeconds;
        }
    }

    private IEnumerator ReturnDropThrough(Blob blob, float lifeTime)
    {
        yield return new WaitForSeconds(lifeTime);
        BlobDisabled.Invoke(blob);
        Release(blob);
    }
}