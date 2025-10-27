using Assets.MultiRougeLike.HexMap.Scripts;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    HexMapPlayer player;
    [SerializeField]
    HexMapGenerator generator;
    List<HexMapCell> cells;
    bool moving = false;
    HashSet<(int, int)> blocked = new HashSet<(int, int)>();
    [SerializeField]
    CanvasController canvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        (player,cells) = generator.Generate(1);
        player.x = 1;
        player.z = 1;
        foreach (var cell in cells)
        {
            if (cell.blocked)
            {
                blocked.Add(cell.Position);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&!moving)
        {
           Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject.GetComponent<HexMapCell>() != null)
                {
                    HexMapCell cell = hitInfo.collider.gameObject.GetComponent<HexMapCell>();
                    print(cell.Position + "StartPoint");
                    StartCoroutine(PathTo(cell.Position));
                }
            }
        }
    }
    public IEnumerator PathTo((int x, int z) endPoint)
    {
        moving = true;
        bool IsWalkable((int x, int z) pos) => !blocked.Contains(pos);
        List<(int x, int z)>? values = HexPathfinder.FindPath(player.Position, endPoint, IsWalkable, (1,1,5,5));
        if(values == null)
        {
            print("Путей нет");
            moving = false;

            yield break;
        }
        foreach (var t in values)
        {
            player.MoveTo(t);
            print(t + " - Шаг №" + (values.IndexOf(t)+1));
            yield return new WaitWhile(() => player.move);
            yield return new WaitForSeconds(1);
        }
        player.Position = endPoint;
        moving = false;
        print("Кончил");
        HexMapCell c = cells.Find(cell => cell.Position == endPoint);
        if (c.city)
        {
            canvas.FillData(c.townData.Value);
        }
    }
}
