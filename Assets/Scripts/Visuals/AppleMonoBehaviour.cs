using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles apple visuals
/// </summary>
public class AppleMonoBehaviour : MonoBehaviour
{
    private Apple _apple;

    [SerializeField] private ParticleSystem _particle;

    public void Attach(Apple apple)
    {
        _apple = apple;
        _apple.OnDestroy += Destroy;
    }

    private void Destroy()
    {
        _apple.OnDestroy -= Destroy;
        _apple = null;
        Destroy(gameObject);

        if (_particle != null)
            Instantiate(_particle, transform.position, Quaternion.identity);
    }
}
