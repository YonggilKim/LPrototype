using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using static Define;

public class BackgroundScroller : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private float _speed = 0.15f;
    float _offset;
    bool _enabled = false;

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnGameStateChange -= HandleGameState;
    }

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Managers.Game.OnGameStateChange += HandleGameState;
    }

    // Update is called once per frame
    void Update()
    {
        if (_enabled)
        {
            _offset += Time.deltaTime * _speed;
            _spriteRenderer.material.mainTextureOffset = new Vector2(_offset, 0);
        }
    }

    public void HandleGameState(Define.eGameState newState)
    {
        switch (newState)
        {
            case eGameState.StageReady:
                _enabled = false;
                break;
            case eGameState.Fight:
                _enabled = false; 
                break;
            case eGameState.MoveNext:
                _enabled = true;
                break;
            default:
                //_enabled = false;
                break;
        }
    }
}
