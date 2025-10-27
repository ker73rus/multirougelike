using System;
using System.Collections.Generic;

public static class HexPathfinder
{
    // Directions для odd-r offset сетки
    private static readonly (int x, int z)[][] OffsetDirections = new (int, int)[][]
    {
        // Чётные ряды (z % 2 == 0)
        new (int, int)[]
        {
            (+1, 0), (0, -1), (-1, -1),
            (-1, 0), (-1, +1), (0, +1)
        },
        // Нечётные ряды (z % 2 != 0)
        new (int, int)[]
        {
            (+1, 0), (+1, -1), (0, -1),
            (-1, 0), (0, +1), (+1, +1)
        }
    };

    /// <summary>
    /// Находит кратчайший путь по гексагональной сетке (A*) с ограничением границ.
    /// Работает с odd-r offset координатами.
    /// </summary>
    public static List<(int x, int z)>? FindPath(
        (int x, int z) startPoint,
        (int x, int z) endPoint,
        Func<(int x, int z), bool> isWalkable,
        (int minX, int minZ, int maxX, int maxZ) bounds)
    {
        // Проверка проходимости и границ
        bool InBounds((int x, int z) p)
            => p.x >= bounds.minX && p.x <= bounds.maxX && p.z >= bounds.minZ && p.z <= bounds.maxZ;

        if (!InBounds(startPoint) || !InBounds(endPoint)) return null;
        if (!isWalkable(startPoint) || !isWalkable(endPoint)) return null;

        // Эвристика (манхэттен для offset-гексов)
        int Heuristic((int x, int z) a, (int x, int z) b)
            => Math.Abs(a.x - b.x) + Math.Abs(a.z - b.z);

        var openSet = new List<(int x, int z)> { startPoint };
        var cameFrom = new Dictionary<(int, int), (int, int)>();
        var gScore = new Dictionary<(int, int), int> { [startPoint] = 0 };
        var fScore = new Dictionary<(int, int), int> { [startPoint] = Heuristic(startPoint, endPoint) };

        while (openSet.Count > 0)
        {
            // Находим узел с минимальным fScore
            (int x, int z) current = openSet[0];
            foreach (var node in openSet)
                if (fScore.GetValueOrDefault(node, int.MaxValue) < fScore.GetValueOrDefault(current, int.MaxValue))
                    current = node;

            // Если дошли до цели — восстанавливаем путь
            if (current.Equals(endPoint))
            {
                var path = new List<(int, int)> { current };
                while (cameFrom.TryGetValue(current, out var prev))
                {
                    current = prev;
                    path.Add(current);
                }
                path.Reverse();
                return path;
            }

            openSet.Remove(current);

            // Определяем чётность ряда
            int parity = current.z % 2 != 0 ? 0 : 1;

            // Перебираем соседей
            foreach (var (dx, dz) in OffsetDirections[parity])
            {
                var neighbor = (current.x + dx, current.z + dz);
                if (!InBounds(neighbor)) continue;
                if (!isWalkable(neighbor)) continue;

                int tentativeG = gScore[current] + 1;
                if (!gScore.TryGetValue(neighbor, out int existingG) || tentativeG < existingG)
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, endPoint);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // путь не найден
    }
}
