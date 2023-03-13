using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles visuals and player interactions with a snake
/// </summary>
public class SnakeMonoBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _segment;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private PlayerInput _playerInput;

    private Snake _snake;

    private readonly Dictionary<Vector2Int, GameObject> _segments = new();

    private Color _color;

    public void AttachSnake(Snake snake)
    {
        _playerInput.Init(snake.Id);
        _playerInput.OnPlayerMove += (direction) =>
        {
            snake.NextDirection = direction;
        };

        _snake = snake;

        _snake.OnHeadAdded += AddNode;
        _snake.OnTailRemoved += RemoveNode;
        _snake.OnDestroy += Destroy;

        AddNode(_snake.Head);
        AddNode(_snake.Tail);
    }

    private void AddNode(Vector2Int pos)
    {
        if (!_segments.ContainsKey(pos))
        {
            var go = Instantiate(_segment, new Vector3(pos.x, 0, pos.y), Quaternion.identity, transform);
            if (go.TryGetComponent<Renderer>(out var renderer))
            {
                renderer.material.color = _color;
            }
            _segments.Add(pos, go);
        }
    }

    private void RemoveNode(Vector2Int pos)
    {
        if (_segments.ContainsKey(pos))
        {
            var go = _segments[pos];
            Destroy(go);
            _segments.Remove(pos);
        }
    }

    private void Destroy()
    {
        _snake.OnHeadAdded -= AddNode;
        _snake.OnTailRemoved -= RemoveNode;
        _snake.OnDestroy -= Destroy;

        _snake = null;

        Destroy(gameObject);

        foreach (var segment in _segments)
        {
            var pos = segment.Key;
            var particle = Instantiate(_particle, new Vector3(pos.x, 0, pos.y), Quaternion.identity);
            if (particle.TryGetComponent<ParticleSystemRenderer>(out var renderer))
            {
                renderer.material.color = _color;
            }
        }
    }

    public void SetColor(Color color)
    {
        _color = color;
    }
}
