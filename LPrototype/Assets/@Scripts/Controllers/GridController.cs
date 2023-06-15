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

    public void CalcCreaturesTargets()
    {
        List<CreatureController> allCreatures = new List<CreatureController>();
        allCreatures.AddRange(Managers.Object.Friends);
        allCreatures.AddRange(Managers.Object.Monsters);

        foreach (CreatureController creature in allCreatures)
        {
            Vector3 dest = GetNextPosition(creature, creature.AtkRange);

            //creature
        }
    }

    public Vector3 GetNextPosition(CreatureController target, float range)
    {
        //List<CreatureController> objects = new List<CreatureController>();

        //List<Vector3> positions = new List<Vector3>();

        //Vector3 targetPos = target.transform.position;

        //Vector3Int left = _grid.WorldToCell(targetPos + new Vector3(-range, 0));
        //Vector3Int right = _grid.WorldToCell(targetPos + new Vector3(+range, 0));
        //Vector3Int center = _grid.WorldToCell(targetPos + new Vector3(-1, 0));
        //Vector3Int bottom = _grid.WorldToCell(targetPos + new Vector3(0, -range));
        //Vector3Int top = _grid.WorldToCell(targetPos + new Vector3(0, +range));

        //int minX = left.x;
        //int maxX = right.x;
        //int minY = bottom.y;
        //int maxY = top.y;

        //switch (target.ObjectType)
        //{
        //    case Define.ObjectType.Player:
        //    case Define.ObjectType.Friend:
        //        minX = (_grid.WorldToCell(targetPos + new Vector3(1, 0))).x;
        //        maxX = right.x;
        //        break;
        //    case Define.ObjectType.Monster:
        //    case Define.ObjectType.Boss:
        //        minX = left.x;
        //        maxX = (_grid.WorldToCell(targetPos + new Vector3(-1, 0))).x;
        //        break;
        //}

        //for (int x = minX; x <= maxX; x++)
        //{
        //    for (int y = minY; y <= maxY; y++)
        //    {
        //        if (_dicCreature.ContainsKey(new Vector3Int(x, y, 0)) == false)
        //        {
        //            //CreatureController 없으면 월드좌표 리턴
        //            Vector3Int cellPosition = new Vector3Int(x, y, 0);
        //            return _grid.CellToWorld(cellPosition);
        //        }
        //        else
        //        {
        //            _dicCreature.TryGetValue(new Vector3Int(x, y, 0), out Cell value);
        //            Debug.Log(value.Creatures.ToList()[0]);
        //        }
        //    }
        //}
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        //if (_grid == null)
        //    return;

        //Gizmos.color = Color.green;

        //Vector3 gridSize = _grid.cellSize;

        //for (int i = -20; i < 20; i++)
        //{
        //    for (int j = -20; j < 20; j++)
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
