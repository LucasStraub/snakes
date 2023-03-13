using System;
using UnityEngine;

[Serializable]
public class SnakeSettings
{
    public Vector2Int InitialPosition => _initialPosition;
    [SerializeField] private Vector2Int _initialPosition;
    public Vector2Int InitialDirection => _initialDirection;
    [SerializeField] private Vector2Int _initialDirection;
    public Color Color => _color;
    [SerializeField] private Color _color = Color.white;
}