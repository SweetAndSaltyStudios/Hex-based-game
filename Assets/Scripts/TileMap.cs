using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileMap
{
    private readonly Tile[,] tiles;

    private readonly int numColumns;
    private readonly int numRows;

    public bool AllowWrapHorizontally = true;
    public bool AllowWrapVertically = true;

    public Tile GetTileAt(int x, int y)
    {
        if (tiles == null)
        {
            Debug.LogError("Hexes array not yet instantiated.");
            return null;
        }

        if (AllowWrapHorizontally)
        {
            x = x % numColumns;
            if (x < 0)
            {
                x += numColumns;
            }
        }
        if (AllowWrapVertically)
        {
            y = y % numRows;
            if (y < 0)
            {
                y += numRows;
            }
        }

        try
        {
            return tiles[x, y];
        }
        catch
        {
            Debug.LogError("GetHexAt: " + x + "," + y);
            return null;
        }
    }

    public TileMap(int numColumns, int numRows)
    {    
        this.numColumns = numColumns;
        this.numRows = numRows;

        tiles = new Tile[numColumns, numRows];
    }

    public void UpdateTilePositions()
    {
        if (MapGenerator.Instance.IsWorldCreated == false)
            return;

        for (int x = 0; x < numColumns; x++)
        {
            for (int y = 0; y < numRows; y++)
            {              
                tiles[x, y].UpdatePosition(CameraEngine.Instance.CurrentPosition, numColumns, numRows);
            }
        }
    }

    public void AddTile(Tile newTile, int column, int row)
    {    
        tiles[column, row] = newTile;
    }

    public void ElevateArea(int column, int row, int radius, float centerHeight = 0.5f)
    {
        var centerTile = GetTileAt(column, row);
        Debug.LogError(centerTile.Column + " , " + centerTile.Row);
        var areaTiles = GetTilesWithInRadius(centerTile, radius);

        foreach (var tile in areaTiles)
        {
            if(tile.Elevation < 0)
            {
                tile.Elevation = 0;
            }

            tile.Elevation +=centerHeight * Mathf.Lerp(1f, 0.25f, Tile.Distance(centerTile, tile) / radius);
        }
    }

    public void UpdateVisuals()
    {
        var material_1 = MapGenerator.Instance.Grasslands_Mat;
        var materail_2 = MapGenerator.Instance.Oceans_Mat;

        for (int x = 0; x < numColumns; x++)
        {
            for (int y = 0; y < numRows; y++)
            {
                var tile = tiles[x, y];
                tile.MeshRenderer.material = tile.Elevation >= 0 ? material_1 : materail_2; 
            }
        }
    }
}
