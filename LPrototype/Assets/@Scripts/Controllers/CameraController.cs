using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera _camera = null;

    #region ī�޶� ��輱 ��ǥ & SIZE
    private float _sizeY;
    private float _sizeX;
    public float Bottom => (_sizeY * -1) + transform.position.y;
    public float Top => _sizeY + transform.position.y;
    public float Left => (_sizeX * -1) + transform.position.x;
    public float Right => _sizeX + transform.position.x;
    public float Height => _sizeY * 2;
    public float Width => _sizeX * 2;
    #endregion

    void Awake()
    {
        _camera = GetComponent<Camera>();
        Managers.Game.Camera = this;
        _sizeY = _camera.orthographicSize;
        _sizeX = _camera.orthographicSize * Screen.width / Screen.height;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            string msg =
                string.Format("��ũ�� ��ǥ: ({0}, {1}) ~ ({2}, {3}) : {4} x {5}",
                    Left, Top, Right, Bottom, Width, Height
                );

            Debug.Log(msg);
        }
    }
}
