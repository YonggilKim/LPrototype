using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class Map : MonoBehaviour
{
    public GridController Grid;//¿Þ ¿À À§ ¾Æ·¡
    UnityEngine.Grid _grid;


    public void Awake()
    {
        Grid = gameObject.GetOrAddComponent<GridController>();
        _grid = gameObject.GetOrAddComponent<UnityEngine.Grid>();
        _grid.cellSize = new Vector3 (Define.CELLSIZE, Define.CELLSIZE, 0);
        Managers.Game.CurrentMap = this;
    }


    public void ChangeMapSize(float targetRate, float time = 120)
    {
        //Vector3 currentSize = Vector3.one * 20f;
        //if (Managers.Game.CurrentWaveIndex > 7)
        //    return;
        //Demarcation.transform.DOScale(currentSize * (10 - Managers.Game.CurrentWaveIndex) * 0.1f, 3);
    }


}
