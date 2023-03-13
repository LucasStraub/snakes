using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple
{
    public Vector2Int Position => _position;

    public bool Destroyed => _destroyed;
    private bool _destroyed = false;

    public Action OnDestroy;

    private Vector2Int _position;

    public Apple(Vector2Int position)
    {
        _position = position;
    }

    public void Destroy()
    {
        _destroyed = true;
        OnDestroy?.Invoke();
    }
}