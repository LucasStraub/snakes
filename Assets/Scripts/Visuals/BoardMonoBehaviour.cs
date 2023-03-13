using System.Threading;
using UnityEngine;

/// <summary>
/// Handles board visuals
/// </summary>
public class BoardMonoBehaviour : MonoBehaviour
{
    [Header("Color")]
    [SerializeField] private Color _boardColor;
    [SerializeField] private Color _bondryColor;

    private Renderer _ground;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _ground = GetComponent<Renderer>();
    }

    public void SetBoundries(Board board)
    {
        if (_ground == null || _camera == null)
            return;

        if (board == null || board.IsInfinity())
        {
            _camera.backgroundColor = _boardColor;

            _ground.gameObject.SetActive(false);
            return;
        }

        _camera.backgroundColor = _bondryColor;

        _ground.material.color = _boardColor;
        _ground.gameObject.SetActive(true);

        _ground.transform.localPosition = new Vector3(board.Center.x, 0, board.Center.y);
        _ground.transform.localScale = new Vector3(board.Size.x, 0, board.Size.y) / 10;
    }
}
