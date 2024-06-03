using UnityEngine;

public class BombSpawnerView : SpawnerView
{
    [SerializeField] private BombSpawner _bombSpawner;

    private void OnEnable()
    {
        _bombSpawner.ChangedCountObjects += Draw;
    }

    private void OnDisable()
    {
        _bombSpawner.ChangedCountObjects -= Draw;
    }
}