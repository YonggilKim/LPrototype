using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

class Cell
{
    public HashSet<CreatureController> Creatures { get; } = new HashSet<CreatureController>();
}

public class GridController : BaseController
{
    UnityEngine.Grid _grid;

    Dictionary<Vector3Int, Cell> _cells = new Dictionary<Vector3Int, Cell>();

    public override bool Init()
    {
        base.Init();

        _grid = gameObject.GetOrAddComponent<UnityEngine.Grid>();

        return true;
    }

    public void Add(CreatureController go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Creatures.Add(go);
    }

    public void Remove(CreatureController go)
    {
        Vector3Int cellPos = _grid.WorldToCell(go.transform.position);

        Cell cell = GetCell(cellPos);
        if (cell == null)
            return;

        cell.Creatures.Remove(go);
    }

    Cell GetCell(Vector3Int cellPos)
    {
        Cell cell = null;

        if (_cells.TryGetValue(cellPos, out cell) == false)
        {
            cell = new Cell();
            _cells.Add(cellPos, cell);
        }

        return cell;
    }
    public Vector3 GetNextPosition(Vector3 targetPos, float range)
    {
        List<CreatureController> objects = new List<CreatureController>();
        List<Vector3> positions = new List<Vector3>();

        Vector3Int left = _grid.WorldToCell(targetPos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(targetPos + new Vector3(+range, 0));
        Vector3Int center = _grid.WorldToCell(targetPos + new Vector3(-1, 0));
        Vector3Int bottom = _grid.WorldToCell(targetPos + new Vector3(0, -range));
        Vector3Int top = _grid.WorldToCell(targetPos + new Vector3(0, +range));

        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        if (range < 0)// targetPos 왼쪽
        {
            minX = left.x;
            maxX = (_grid.WorldToCell(targetPos + new Vector3(-1, 0))).x;
        }
        else
        {
            minX = (_grid.WorldToCell(targetPos + new Vector3(1, 0))).x;
            maxX = right.x;
        }

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (_cells.ContainsKey(new Vector3Int(x, y, 0)) == false)
                {
                    //CreatureController 없으면 월드좌표 리턴
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    return _grid.GetCellCenterWorld(cellPosition);
                }
            }
        }
        return Vector3.zero;
    }

    public List<CreatureController> GatherObjects(Vector3 pos, float range)
    {
        List<CreatureController> objects = new List<CreatureController>();

        Vector3Int left = _grid.WorldToCell(pos + new Vector3(-range, 0));
        Vector3Int right = _grid.WorldToCell(pos + new Vector3(+range, 0));
        Vector3Int bottom = _grid.WorldToCell(pos + new Vector3(0, -range));
        Vector3Int top = _grid.WorldToCell(pos + new Vector3(0, +range));

        int minX = left.x;
        int maxX = right.x;
        int minY = bottom.y;
        int maxY = top.y;

        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (_cells.ContainsKey(new Vector3Int(x, y, 0)) == false)
                    continue;

                objects.AddRange(_cells[new Vector3Int(x, y, 0)].Creatures);
            }
        }

        return objects;
    }
}
