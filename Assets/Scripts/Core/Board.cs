using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoardSettings
{
    public Vector2Int Size => _size;
    [SerializeField] private Vector2Int _size;

    public Vector2Int Center => _center;
    [SerializeField] private Vector2Int _center;

    public bool IsInfinity()
    {
        return _size.x * _size.y <= 0;
    }
}

public class Board
{
    public Vector2Int Size => _size;
    [SerializeField] private Vector2Int _size;

    public Vector2 Center
    {
        get
        {
            var offset = Vector2.zero;
            if (_size.x % 2 == 0)
            {
                offset.x = 0.5f;
            }
            if (_size.y % 2 == 0)
            {
                offset.y = 0.5f;
            }
            return _center + offset;
        }
    }
    [SerializeField] private Vector2Int _center;

    private HashSet<Vector2Int> _boardHashSet;

    public Board(Vector2Int size, Vector2Int center)
    {
        _size = size;
        _center = center;
    }

    /// <summary>
    /// Gets board area
    /// </summary>
    /// <returns></returns>
    public int Area()
    {
        if (IsInfinity())
        {
            return int.MaxValue;
        }
        return _size.x * _size.y;
    }

    /// <summary>
    /// Checks if a position is inbouds of the board
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsInbonds(Vector2Int position)
    {
        if (IsInfinity())
        {
            return true;
        }

        var maxBounds = Center + _size / 2;
        var minBounds = Center - _size / 2;

        return position.x <= maxBounds.x &&
                position.x >= minBounds.x &&
                position.y <= maxBounds.y &&
                position.y >= minBounds.y;
    }

    /// <summary>
    /// Checks if board has infinity size
    /// </summary>
    /// <returns></returns>
    public bool IsInfinity()
    {
        return _size.x * _size.y <= 0;
    }

    public HashSet<Vector2Int> GetBoardHashSet()
    {
        if (_boardHashSet == null)
        {
            _boardHashSet = new HashSet<Vector2Int>();

            if (!IsInfinity())
            {
                for (int i = -_size.x / 2; i < _size.x / 2; i++)
                {
                    for (int j = -_size.y / 2; j < _size.y / 2; j++)
                    {
                        var position = new Vector2Int(i, j) + _center;
                        _boardHashSet.Add(position);
                    }
                }
            }
        }
        return _boardHashSet;
    }
}