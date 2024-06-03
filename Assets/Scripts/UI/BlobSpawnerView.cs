using UnityEngine;

public class BlobSpawnerView : SpawnerView
{
    [SerializeField] private BlobSpawner _blobSpawner;

    private void OnEnable()
    {
        _blobSpawner.ChangedCountObjects += Draw;
    }

    private void OnDisable()
    {
        _blobSpawner.ChangedCountObjects -= Draw;
    }
}