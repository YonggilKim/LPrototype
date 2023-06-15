using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    public float speed;
    float _offset;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Managers.Game.Player.CreatureState == Define.eCreatureState.Moving)
        //{ 
        //    _offset += Time.deltaTime * 0.3f;
        //    _spriteRenderer.material.mainTextureOffset = new Vector2(_offset, 0);
        //}
    }
}
