using System;
using System.Collections.Generic;
using UnityEngine;

public class Snake
{
    public Action<Vector2Int> OnHeadAdded;
    public Action<Vector2Int> OnTailRemoved;
    public Action OnDestroy;

    public int Id => _id;
    private readonly int _id = 0;

    public Vector2Int NextHead;

    public Vector2Int Head => _head;
    private Vector2Int _head = Vector2Int.zero;

    public Vector2Int NextDirection;
    public Vector2Int Direction => _direction;
    private Vector2Int _direction = Vector2Int.up;

    public Vector2Int Tail => _bodyQueue.Peek();

    public bool Destroyed => _destroyed;

    public int Length => _bodyHashSet.Count;

    private bool _destroyed;

    public HashSet<Vector2Int> BodyHashSet => _bodyHashSet;
    private readonly HashSet<Vector2Int> _bodyHashSet = new(); // Used for faster collision checks
    private readonly Queue<Vector2Int> _bodyQueue = new(); // Used to queue body parts

    /// <summary>
    /// Creates new snake if given position and direction
    /// </summary>
    /// <param name="position"></param>
    /// <param name="direction"></param>
    /// <param name="id"></param>
    public Snake(Vector2Int position, Vector2Int direction, int id)
    {
        _id = id;

        AddHead(position - direction);
        AddHead(position);

        Debug.Log($"{this} spawned with head at {Head} and tail at {Tail}");
    }

    /// <summary>
    /// Adds a new head, making the old one part of the body
    /// </summary>
    /// <param name="position"></param>
    public void AddHead(Vector2Int position)
    {
        var lastPostion = Head;
        _direction = position - lastPostion;
        NextDirection = _direction;

        _head = position;

        _bodyQueue.Enqueue(position);
        _bodyHashSet.Add(position);

        OnHeadAdded?.Invoke(position);

        Debug.Log($"{this} added head at {position}");
    }

    /// <summary>
    /// Remove its tail, making the new tail the next body part
    /// </summary>
    public void RemoveTail()
    {
        var position = _bodyQueue.Dequeue();

        _bodyHashSet.Remove(position);

        OnTailRemoved?.Invoke(position);

        Debug.Log($"{this} removed tail at {position}");
    }

    /// <summary>
    /// Check if a position is colliding with the snake
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool CheckCollision(Vector2Int position)
    {
        return _bodyHashSet.Contains(position);
    }

    /// <summary>
    /// Set this snake to be destroid
    /// </summary>
    public void Destroy()
    {
        _destroyed = true;
        OnDestroy?.Invoke();

        Debug.Log($"{this} setted to be destroyed");
    }

    public override string ToString()
    {
        return $"SNAKE_{_id}";
    }
}