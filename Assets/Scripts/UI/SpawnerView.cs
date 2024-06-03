using System;
using TMPro;
using UnityEngine;

public abstract class SpawnerView<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected Spawner<T> Spawner;
    
    [SerializeField] private string _spawnedObjectName;

    private TMP_Text _text;

    private void Awake()
    {
        if (TryGetComponent(out TMP_Text text))
            _text = text;

        Draw(0, 0);
    }

    private void OnEnable()
    {
        Spawner.OnChangedCountObjects += Draw;
    }

    private void OnDisable()
    {
        Spawner.OnChangedCountObjects -= Draw;
    }

    protected void Draw(int countOfCreatedObjects, int countOfActiveObjects)
    {
        if (_text != null)
            _text.text = $"{_spawnedObjectName} created - {countOfCreatedObjects}, active - {countOfActiveObjects}";
    }
}