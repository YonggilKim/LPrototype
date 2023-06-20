using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GridController : BaseController
{
    UnityEngine.Grid _grid;

    Dictionary<Vector3Int, CreatureController> _dicCreature = new Dictionary<Vector3Int, CreatureController>();

    public override bool Init()
    {
        base.Init();

        _grid = gameObject.GetOrAddComponent<UnityEngine.Grid>();

        return true;
    }
    //목표지점에 도착하면 add하기

    public void Add(CreatureController go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        if (_dicCreature.TryGetValue(cellPos, out CreatureController creatureController) == false)
        {
            _dicCreature.Add(cellPos, go);
        }
    }

    public void Remove(CreatureController go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        if (_dicCreature.TryGetValue(cellPos, out CreatureController creatureController) == true)
        {
            _dicCreature.Remove(cellPos);
        }
    }

    public bool CanAdd(Vector3 woldPos)
    {
        Vector3Int cellPos = _grid.WorldToCell(woldPos);
        if (_dicCreature.TryGetValue(cellPos, out CreatureController creatureController) == true)
            return false;
        else
            return true;
    }


    private void OnDrawGizmos()
    {
        //if (_grid == null)
        //    return;

        //Gizmos.color = Color.green;

        //Vector3 gridSize = _grid.cellSize;

        //for (int i = -20; i < 20; i++)
        //{
        //    for (int j = 0; j < 6; j++)
        //    {
        //        Vector3Int cellPosition = new Vector3Int(i, j, 0);
        //        Vector3 worldPosition = _grid.CellToWorld(cellPosition);
        //        Gizmos.color = (i == 0 && j == 0) ? Color.blue : Color.green;
        //        Gizmos.DrawWireSphere(worldPosition, 0.1f);

        //        string label = string.Format("({0}, {1})", i, j);
        //        GUIStyle style = new GUIStyle();
        //        style.normal.textColor = Color.white;
        //        style.fontSize = 12;
        //        Handles.Label(worldPosition, label, style);

        //        //label = string.Format("World: ({0:F1}, {1:F1}, {2:F1})", worldPosition.x, worldPosition.y, worldPosition.z);
        //        //Handles.Label(worldPosition + new Vector3(0, -0.2f, 0), label, style);

        //    }
        //}
    }

}
