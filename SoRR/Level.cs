using JetBrains.Annotations;
using SoRR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoRR;

public class LevelData
{
    public LevelData(int width, int height)
    {
        Cells = new CellData[height][];
        for (int i = 0; i < height; i++)
        {
            Cells[i] = new CellData[width];
            for (int j = 0; j < width; j++)
                Cells[i][j] = new CellData();
        }
    }
    public CellData[][] Cells { get; private set; } = null!;
    public int Height { get { return Cells.Length; } }
    public int Width{ get { return Cells[0].Length; } }
}
public class CellData
{
    public List<CellDataLayout>? Layouts { get; private set; }
    public List<ObstacleMetadata>? Obstacles { get; private set; }

    public void AddLayout<TLayout>(int index) where TLayout : CellDataLayout, new()
    {
        NonValidIndexCheck(index);

        if (Layouts is null) Layouts = new();
        Layouts.Add(new TLayout());
    }

    [Pure]
    public IEnumerable<TLayout> GetLayouts<TLayout>() where TLayout : CellDataLayout
    {
        if (Layouts is null) throw new ArgumentNullException(nameof(Layouts));
        return Layouts.OfType<TLayout>();
    }

    [Pure]
    public IEnumerable<TLayout> GetLayouts<TLayout>(int index) where TLayout : CellDataLayout
    {
        return GetLayouts<TLayout>().Where(x => x.Index == index);
    }

    [Pure]
    public IEnumerable<TLayout> GetLayouts<TLayout>(string spriteName) where TLayout : CellDataLayout
    {
        return GetLayouts<TLayout>().Where(x => x.SpriteName == spriteName);
    }

    [Pure]
    public IEnumerable<TLayout> GetLayouts<TLayout>(LayoutType type) where TLayout : CellDataLayout
    {
        return GetLayouts<TLayout>().Where(x => x.LayoutType == type);
    }

    [Pure]
    public static void NonValidIndexCheck(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
    }

    public void AddObject<TObstacle>() where TObstacle : Obstacle, new()
    {
        ObstacleMetadata metadata = ObstacleMetadata.Get<TObstacle>();
        if (metadata.CanPlaceOnCell(this))
        {
            if (Obstacles is null) Obstacles = new();
            Obstacles.Add(metadata);
        }
    }

    [Pure]
    public IEnumerable<ObstacleMetadata> GetObjects<T>() where T : Type
    {
        if (Obstacles is null) throw new ArgumentNullException(nameof(Obstacles));
        return Obstacles.Where(i => i.Type == typeof(T));
    }

    [Pure]
    public IEnumerable<ObstacleMetadata> GetObjects<T>(string obstacleName) where T : Type
    {
        return GetObjects<T>().Where(x => x.Name == obstacleName);
    }
}
public abstract class CellDataLayout
{
    public string SpriteName { get; private set; } = null!;
    public LayoutType LayoutType { get; private set; } = LayoutType.Empty;
    public int Index { get; private set; } = 0;
    public void SetSpriteName(string spriteName)
    {
        if (spriteName is null || spriteName == "") throw new ArgumentNullException(nameof(spriteName));
        SpriteName = spriteName;
    }
    public void SetIndex(int index)
    {
        CellData.NonValidIndexCheck(index);
        Index = index;
    }
}
public enum LayoutType : byte
{
    Empty, // aka. Void
    Floor,
    FloorExtra,
}
