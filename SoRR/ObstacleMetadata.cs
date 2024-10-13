using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SoRR;

public sealed class ObstacleMetadata
{
    public Type Type { get; }
    public string Name { get; }

    public Dir4 Direction { get; set; } = Dir4.North;

    public ReadOnlyCollection<string> ConflictingObstacles { get; } = emptyCollection;

    private static readonly ReadOnlyCollection<string> emptyCollection = new(Array.Empty<string>());
    private static readonly Dictionary<Type, ObstacleMetadata> infos = new Dictionary<Type, ObstacleMetadata>();

    public static ObstacleMetadata Get(Type type) => infos.TryGetValue(type, out ObstacleMetadata info) ? info : infos[type] = new ObstacleMetadata(type);
    public static ObstacleMetadata Get<TObstacle>() where TObstacle : Obstacle => Get(typeof(TObstacle));
    public bool CanPlaceOnCell(CellData cell)
    {
        if (ConflictingObstacles is null) return true;

        List<ObstacleMetadata> conflictingObjects = cell.Obstacles.Where(i => i.ConflictingObstacles.Count != 0).ToList();
        if (conflictingObjects.Exists(i => i.ConflictingObstacles.Contains(Name))) return false;

        return true;
    }
    private ObstacleMetadata(Type type, List<string> conflictingObstacles = null)
    {
        Type = type;
        Name = type.Name;
        if (conflictingObstacles != null)
        {
            ConflictingObstacles = new ReadOnlyCollection<string>(conflictingObstacles);
        }
    }
}
