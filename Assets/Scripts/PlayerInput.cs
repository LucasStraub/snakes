using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

/// <summary>
/// Handles player input
/// </summary>
public class PlayerInput : MonoBehaviour
{
    private int _id = 0;

    private Controls _controls;

    public Action<Vector2Int> OnPlayerMove;

    private void OnEnable()
    {
        if (_controls != null)
            _controls.Enable();
    }

    private void OnDisable()
    {
        if (_controls != null)
            _controls.Disable();
    }

    private void OnDestroy()
    {
        if (_controls != null)
            _controls.Dispose();
    }

    public void Init(int id)
    {
        _controls = new Controls();
        _controls.Enable();

        _id = id;

        switch (_id)
        {
            case 0:
                _controls.Player1.Movement.performed += MovePlayer;
                break;
            case 1:
                _controls.Player2.Movement.performed += MovePlayer;
                break;
            case 2:
                _controls.Player3.Movement.performed += MovePlayer;
                break;
            case 3:
                _controls.Player4.Movement.performed += MovePlayer;
                break;
        }
    }

    public void MovePlayer(CallbackContext callbackContext)
    {
        var vector2 = callbackContext.ReadValue<Vector2>();
        vector2 = vector2.normalized;

        var vector2Int = new Vector2Int()
        {
            x = Mathf.RoundToInt(vector2.x),
            y = Mathf.RoundToInt(vector2.y),
        };

        OnPlayerMove?.Invoke(vector2Int);
    }
}
