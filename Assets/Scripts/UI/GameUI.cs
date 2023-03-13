using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Action OnGameStart;

    [SerializeField] private GameObject _startMenu;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _endMenu;
    [SerializeField] private Button _restartButton;

    private void Awake()
    {
        if (_startButton != null)
        {
            _startButton.onClick.AddListener(() => OnGameStart?.Invoke());
        }
        if (_restartButton != null)
        {
            _restartButton.onClick.AddListener(() => OnGameStart?.Invoke());
        }
    }

    public void SetUI(GameManager.GameState gameState)
    {
        if (_startMenu != null)
        {
            _startMenu.SetActive(gameState == GameManager.GameState.Start);
        }
        if (_endMenu != null)
        {
            _endMenu.SetActive(gameState == GameManager.GameState.End);
        }
    }
}
