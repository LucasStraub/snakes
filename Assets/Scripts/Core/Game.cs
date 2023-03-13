using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game
{
    public Action<Apple> OnAppleSpawn;
    public Action OnGameEnd;

    private List<Snake> _snakes = new();
    private Apple _apple;
    private Board _board;

    private bool _running = false;

    /// <summary>
    /// Setup game and starts its loop
    /// </summary>
    /// <param name="board"></param>
    /// <param name="updatesPerSecond"></param>
    /// <param name="snakes"></param>
    /// <returns></returns>
    public IEnumerator Start(Board board, float updatesPerSecond, Snake[] snakes)
    {
        _board = board;

        _snakes = snakes.ToList();

        yield return Loop(updatesPerSecond);
    }

    /// <summary>
    /// Loop snake game logic
    /// </summary>
    /// <param name="updatesPerSecond"></param>
    /// <returns></returns>
    private IEnumerator Loop(float updatesPerSecond)
    {
        _running = true;

        while (_running)
        {
            if (_apple == null)
            {
                SpawnRandomApple();
            }

            yield return new WaitForSeconds(1 / updatesPerSecond);

            var nextHeads = new Dictionary<Vector2Int, Snake>();
            foreach (var snake in _snakes)
            {
                /// Apply player input on the loop if valid
                if (snake.NextDirection == -snake.Direction)
                {
                    snake.NextHead = snake.Head + snake.Direction;
                }
                else
                {
                    snake.NextHead = snake.Head + snake.NextDirection;
                }

                /// Check if next position will be inbounds
                if (!_board.IsInbonds(snake.NextHead))
                {
                    snake.Destroy();
                }

                /// Checks for headh to head collisions
                /// Will mark both snakes are destroiable
                if (nextHeads.ContainsKey(snake.NextHead))
                {
                    snake.Destroy();
                    nextHeads[snake.NextHead].Destroy();
                    Debug.Log($"{snake} hit heads with {nextHeads[snake.NextHead]} at {snake.NextHead}");
                }
                else
                {
                    nextHeads.Add(snake.NextHead, snake);
                }
            }

            /// Checks for general collisions
            /// Its done in another foreach due to size increase and tail interaction
            foreach (var snake in _snakes)
            {
                foreach (var s in _snakes)
                {
                    if (s.CheckCollision(snake.NextHead))
                    {
                        if (snake.NextHead != s.Tail || (s.NextHead == _apple?.Position && !s.Destroyed))
                        {
                            Debug.Log($"{snake} hit with {s} at {snake.NextHead}");
                            snake.Destroy();
                            break;
                        }
                    }
                }
            }

            /// Move snakes by adding a new head and removing tail for it
            /// It will not remove its tail if it has eaten an apple
            foreach (var snake in _snakes)
            {
                snake.AddHead(snake.NextHead);

                if (snake.NextHead == _apple?.Position && !snake.Destroyed)
                {
                    Debug.Log($"{snake} ate apple");

                    _apple.Destroy();
                }
                else
                {
                    snake.RemoveTail();
                }
            }

            /// Creates a new apple if older one was eaten
            if (_apple != null && _apple.Destroyed)
            {
                _apple = null;

                SpawnRandomApple();
            }

            /// Remove destroyed snakes from snake list
            _snakes = _snakes.Where(o => !o.Destroyed).ToList();

            /// Checks for end game conditions
            /// If there's no snakes
            /// If snakes have reched board limit
            if (_snakes.Count == 0 || _snakes.Sum(o => o.Length) == _board.Area())
            {
                _running = false;

                OnGameEnd?.Invoke();
            }
        }
    }

    /// <summary>
    /// Spawns a new random apple
    /// </summary>
    private void SpawnRandomApple()
    {
        var position = Vector2Int.zero;

        if (!_board.IsInfinity())
        {
            /// Gets a random position based on 
            var availablePositions = _board.GetBoardHashSet();

            for (int i = 0; i < _snakes.Count; i++)
            {
                availablePositions.ExceptWith(_snakes[i].BodyHashSet);
            }

            int random = Random.Range(0, availablePositions.Count);

            position = availablePositions.ToArray()[random];
        }
        else
        {
            // TODO find availiable position for a infinity board, not to far from players
        }

        if (_board.IsInbonds(position))
        {
            _apple = new Apple(position);
            OnAppleSpawn?.Invoke(_apple);
        }
        else
        {
            Debug.LogWarning("Can not spawn Apple outside bounds");
        }
    }
}