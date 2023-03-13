using System;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Game _gameManager;
    [SerializeField] private GameUI _gameUI;

    [SerializeField] private BoardMonoBehaviour _boardMonoBehaviour;

    [Header("Prefabs")]
    public SnakeMonoBehaviour SnakePrefab;
    public AppleMonoBehaviour ApplePrefab;

    [Header("Settings")]
    [SerializeField] private float _updatesPerSecond = 1f;
    [SerializeField] private BoardSettings _boardSettings;
    [SerializeField] private SnakeSettings[] _snakeSettings = new SnakeSettings[] { };

    private Coroutine _game;

    public enum GameState
    {
        Start = 0,
        Running = 1,
        End = 2,
    }

    private void Awake()
    {
        _gameManager = new Game();

        _gameManager.OnAppleSpawn += SpawnApple;
        _gameManager.OnGameEnd += EndGame;

        _gameUI.OnGameStart += GameStart;
    }

    private void Start()
    {
        _gameUI.SetUI(GameState.Start);
    }

    private void OnDestroy()
    {
        EndGame();
    }

    private void OnDrawGizmos()
    {
        // Shows board size on editor scene
        if (!_boardSettings.IsInfinity()) // Checks if not infinity board
        {
            Gizmos.DrawWireCube(new Vector3(_boardSettings.Center.x, 0, _boardSettings.Center.y), new Vector3(_boardSettings.Size.x, 0, _boardSettings.Size.y));
        }
    }

    private void GameStart()
    {
        var board = SetBoard(_boardSettings);
        var snakes = SpawnSnakes(_snakeSettings);

        _game = StartCoroutine(_gameManager.Start(board, _updatesPerSecond, snakes));

        _gameUI.SetUI(GameState.Running);
    }

    private void SpawnApple(Apple apple)
    {
        var appleMonoBehaviour = Instantiate(ApplePrefab, new Vector3(apple.Position.x, 0, apple.Position.y), Quaternion.identity);
        appleMonoBehaviour.Attach(apple);
    }

    private Board SetBoard(BoardSettings boardSettings)
    {
        var board = new Board(boardSettings.Size, boardSettings.Center);
        _boardMonoBehaviour.SetBoundries(board);

        return board;
    }

    private Snake[] SpawnSnakes(SnakeSettings[] snakeSettings)
    {
        var snakes = new Snake[snakeSettings.Length];
        for (int i = 0; i < snakeSettings.Length; i++)
        {
            var playerSettings = snakeSettings[i];

            var snake = new Snake(playerSettings.InitialPosition, playerSettings.InitialDirection, i);
            snakes[i] = snake;

            var snakeMonobehaviour = Instantiate(SnakePrefab);
            snakeMonobehaviour.SetColor(playerSettings.Color);
            snakeMonobehaviour.AttachSnake(snake);
        }
        return snakes;
    }

    public void EndGame()
    {
        StopCoroutine(_game);

        _gameUI.SetUI(GameState.End);
    }
}