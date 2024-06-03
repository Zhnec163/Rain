using System;
using UnityEngine;

public class Blob : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Color _defaultColor;
    private bool _isDropped;
    private Action<Blob> _dropped;

    private void OnDestroy()
    {
        _dropped = null;
    }
    
    public void Init(Action<Blob> dropped)
    {
        _dropped = dropped;
        
        if (TryGetComponent(out MeshRenderer meshRenderer))
        {
            _meshRenderer = meshRenderer;
            _defaultColor = _meshRenderer.material.color;
        }
    }

    public void ResetState()
    {
        _isDropped = false;
        SetDefaultColor();
    }

    private void SetDefaultColor()
    {
        _meshRenderer.material.color = _defaultColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isDropped)
            return;
        
        if (collision.gameObject.TryGetComponent(out Ground ground))
        {
            _isDropped = true;
            _meshRenderer.material.color = RandomHelper.GetRandomColor();
            _dropped.Invoke(this);
        }
    }
}
