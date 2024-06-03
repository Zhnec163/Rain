using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(SphereCollider))]
public class Bomb : MonoBehaviour
{
    [SerializeField] private float _explosionForce = 400.0f;
    [SerializeField] private float _radius = 20.0f;

    private MeshRenderer _meshRenderer;
    private SphereCollider _collider;
    private int _lifetime;

    public event Action<Bomb> Exploded;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<SphereCollider>();
    }

    private void OnDisable()
    {
        Exploded = null;
    }

    public void Init(Action<Bomb> exploded)
    {
        Exploded = exploded;
    }

    public void Activate(float speedColorChange)
    {
        StartCoroutine(ColorChanging(speedColorChange));
    }

    public IEnumerator ColorChanging(float speed)
    {
        while (_meshRenderer.material.color.a > 0)
        {
            float a = Mathf.MoveTowards(_meshRenderer.material.color.a, 0, speed * Time.deltaTime);
            _meshRenderer.material.color = new Color(_meshRenderer.material.color.r, _meshRenderer.material.color.g,
                _meshRenderer.material.color.b, a);
            yield return null;
        }

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(_lifetime);

        if (TryGetExplosionObjects(out List<Rigidbody> rigidbodies))
        {
            foreach (var explosionObject in rigidbodies)
                explosionObject.AddExplosionForce(_explosionForce, transform.position, _radius);
        }

        Exploded?.Invoke(this);
    }

    private bool TryGetExplosionObjects(out List<Rigidbody> rigidbodies)
    {
        rigidbodies = new List<Rigidbody>();

        List<Collider> colliders = Physics.OverlapSphere(transform.position, _radius).ToList();
        colliders.Remove(_collider);

        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent(out Rigidbody rigidbody))
                rigidbodies.Add(rigidbody);
        }

        return rigidbodies.Count > 0;
    }
}