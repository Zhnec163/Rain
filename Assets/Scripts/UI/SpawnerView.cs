using TMPro;
using UnityEngine;

public class SpawnerView : MonoBehaviour
{
    [field: SerializeField] private string _spawnedObjectName;

    private TMP_Text _text;

    private void Awake()
    {
        if (TryGetComponent(out TMP_Text text))
            _text = text;

        Draw(0, 0);
    }

    protected void Draw(int countOfCreatedObjects, int countOfActiveObjects)
    {
        if (_text != null)
            _text.text = $"{_spawnedObjectName} created - {countOfCreatedObjects}, active - {countOfActiveObjects}";
    }
}