using Assets.MultiRougeLike.HexMap.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HexMapGenerator : MapGenerator
{
    float outerr = 0;
    float innerr = 1.75f;
    [SerializeField]
    GameObject PlayerPrefab;
    public (HexMapPlayer, List<HexMapCell>) Generate(int seed)
    {
        Random.InitState(seed);
        List<HexMapCell> cells = new List<HexMapCell>();
        outerr = innerr / 0.866025404f;

        // 1️⃣ — создаём все клетки как проходимые
        HexMapCell[,] map = new HexMapCell[width + 1, height + 1];
        for (int i = 1; i <= width; i++)
        {
            for (int j = 1; j <= height; j++)
            {
                float x = i * innerr;
                if (j % 2 == 0) x += innerr / 2;
                float z = j * outerr - (outerr - innerr) * 1.9f * j;

                int index = 1; // 1 = проходимая ячейка
                if(i == width && j == height)
                {
                    index = 2;
                }
                GameObject t = Instantiate(CellPrefab[index], new Vector3(x, 0, z), Quaternion.identity);
                HexMapCell cell = t.GetComponent<HexMapCell>();
                cell.blocked = false; 
                if (i == width && j == height)
                {
                    cell.city = true;
                }
                cell.Set(i, j);

                cells.Add(cell);
                map[i, j] = cell;
            }
        }

        // 2️⃣ — случайно блокируем часть ячеек (кроме стартовой)
        int maxBlocked = (int)(width * height * 0.25f); // 25% максимум
        int blockedCount = 0;

        while (blockedCount < maxBlocked)
        {
            int x = Random.Range(1, width + 1);
            int z = Random.Range(1, height + 1);
            var cell = map[x, z];
            if (cell == null || cell.blocked || (x == 1 && z == 1) || cell.city) continue;

            // временно блокируем
            cell.blocked = true;

            // Проверяем связность
            if (!IsMapFullyConnected(map, width, height))
            {
                // если карта распалась — откатываем
                cell.blocked = false;
                continue;
            }

            // иначе — оставляем
            blockedCount++;
            float i = x * innerr;
            if (z % 2 == 0) i += innerr / 2;
            float j = z * outerr - (outerr - innerr) * 1.9f * z;
            var t = Instantiate(CellPrefab[0], new Vector3(i, 0, j), Quaternion.identity).GetComponent<HexMapCell>();
            t.blocked = true;
            t.Set(x, z);
            cells[cells.IndexOf(cell)] = t;
            Destroy(cell.gameObject);
        }

        // 3️⃣ — создаём игрока
        var player = Instantiate(PlayerPrefab, new Vector3(innerr, 0.5f, 1.5f), Quaternion.identity)
            .GetComponent<HexMapPlayer>();

        return (player, cells);
    }
    private bool IsMapFullyConnected(HexMapCell[,] map, int width, int height)
    {
        var visited = new HashSet<(int, int)>();
        var queue = new Queue<(int, int)>();

        // находим первую непроходимую клетку
        (int, int)? start = null;
        for (int i = 1; i <= width && start == null; i++)
            for (int j = 1; j <= height; j++)
                if (!map[i, j].blocked)
                {
                    start = (i, j);
                    break;
                }

        if (start == null) return false;

        queue.Enqueue(start.Value);
        visited.Add(start.Value);

        // стандартные соседи (odd-r offset)
        (int, int)[][] dirs = new (int, int)[][]
        {
            new (int, int)[]{ (+1,0), (0,-1), (-1,-1), (-1,0), (-1,+1), (0,+1) },
            new (int, int)[]{ (+1,0), (+1,-1), (0,-1), (-1,0), (0,+1), (+1,+1) }
        };

        while (queue.Count > 0)
        {
            var (cx, cz) = queue.Dequeue();
            int parity = cz % 2 == 1 ? 0 : 1;

            foreach (var (dx, dz) in dirs[parity])
            {
                int nx = cx + dx;
                int nz = cz + dz;

                if (nx < 1 || nz < 1 || nx > width || nz > height)
                    continue;

                var neighbor = map[nx, nz];
                if (neighbor.blocked || visited.Contains((nx, nz)))
                    continue;

                visited.Add((nx, nz));
                queue.Enqueue((nx, nz));
            }
        }

        // Проверяем — все ли проходимые клетки мы обошли
        for (int i = 1; i <= width; i++)
            for (int j = 1; j <= height; j++)
                if (!map[i, j].blocked && !visited.Contains((i, j)))
                    return false;

        return true;
    }
}
