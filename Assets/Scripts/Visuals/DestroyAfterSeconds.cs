using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [SerializeField] private float _delay = 1f;

    private void Start()
    {
        Invoke(nameof(Destroy), _delay);
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}
